using System;
using System.Collections.Generic;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class ShiftRecommendationOption
    {
        public User CandidateUser { get; set; } = new User();
        public DateTime ShiftDate { get; set; }
        public string ShiftTypeName { get; set; } = string.Empty;
        public int HistoricalShiftCount { get; set; }
        public double FairnessScore { get; set; }
        public double SortScore { get; set; }
        public bool IsOnLeaveOnDate { get; set; }
        public bool IsOnShiftOnDate { get; set; }
        public string RecommendationReason { get; set; } = string.Empty;

        public string TeamBadgeText => string.IsNullOrWhiteSpace(CandidateUser.Team) ? CandidateUser.Title : $"{CandidateUser.Team} Ekibi • {CandidateUser.Title}";
        public string BadgeText => $"🏆 Adillik Puanı: %{Math.Min(100.0, Math.Max(0.0, FairnessScore)):F0}";
        public string StatusBadge => IsOnLeaveOnDate
            ? "⚠️ Personel O Tarihte İzinde!"
            : IsOnShiftOnDate
                ? "⚠️ O Tarihte Zaten Nöbetçi!"
                : "✅ Nöbet ve İzin Durumu Uygun";
    }

    public class SmartShiftAssistantService
    {
        public List<ShiftRecommendationOption> GetShiftRecommendationsForDate(
            DateTime targetDate,
            string targetTeamName,
            IEnumerable<User> allUsers,
            IEnumerable<Shift> allShifts,
            IEnumerable<Leave> allLeaves,
            IEnumerable<CustomShift>? allCustomShifts = null)
        {
            var results = new List<ShiftRecommendationOption>();
            if (allUsers == null) return results;

            // Filter users by team if specified (and not "Tüm Ekipler")
            var candidateUsers = allUsers.Where(u => !u.IsAdmin).ToList();

            if (!string.IsNullOrWhiteSpace(targetTeamName) && !string.Equals(targetTeamName, "Tüm Ekipler", StringComparison.OrdinalIgnoreCase))
            {
                var teamFiltered = candidateUsers.Where(u => string.Equals(u.Team, targetTeamName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (teamFiltered.Any())
                    candidateUsers = teamFiltered;
            }

            var activeLeaves = allLeaves?.Where(l => l.Status != "Rejected" && l.Status != "Reddedildi").ToList() ?? new List<Leave>();
            var existingShifts = allShifts?.ToList() ?? new List<Shift>();
            var customShiftList = allCustomShifts?.ToList() ?? new List<CustomShift>();

            foreach (var user in candidateUsers)
            {
                // Total regular shift count for user
                int regularShiftCount = existingShifts.Count(s => s.UserId == user.Id);

                // Total custom shift count where user is assigned (by name)
                int customShiftCount = customShiftList.Count(cs =>
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                int shiftCount = regularShiftCount + customShiftCount;

                // Is user on leave on target date?
                bool onLeave = activeLeaves.Any(l =>
                    (l.UserId == user.Id || (l.User != null && string.Equals(l.User.FullName, user.FullName, StringComparison.OrdinalIgnoreCase))) &&
                    l.StartDate.Date <= targetDate.Date && l.EndDate.Date >= targetDate.Date);

                // Does user already have a custom shift ON the target date?
                bool onCustomShiftToday = customShiftList.Any(cs =>
                    cs.ShiftDate.Date == targetDate.Date &&
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                // Has user worked a shift in the last 7 days (regular or custom)?
                bool workedRecentlyRegular = existingShifts.Any(s =>
                    s.UserId == user.Id && Math.Abs((s.ShiftDate.Date - targetDate.Date).TotalDays) <= 7);

                bool workedRecentlyCustom = customShiftList.Any(cs =>
                    Math.Abs((cs.ShiftDate.Date - targetDate.Date).TotalDays) <= 7 &&
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                bool workedRecently = workedRecentlyRegular || workedRecentlyCustom;

                // Base fairness score
                double baseScore = 100.0 - (shiftCount * 12.0);
                if (workedRecently) baseScore -= 25.0;
                if (onLeave) baseScore -= 60.0;
                if (onCustomShiftToday) baseScore -= 80.0; // Already on shift today

                double displayScore = Math.Min(100.0, Math.Max(5.0, baseScore));
                double sortScore = displayScore - (((user.Id * 7 + targetDate.DayOfYear * 13) % 11) * 0.05);

                string reason;
                if (onLeave)
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi) bu tarihte izinli olduğu için önerilmiyor.";
                }
                else if (onCustomShiftToday)
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi • {user.Title}) bu tarihte zaten özel nöbetçi. Toplam nöbet sayısı: {shiftCount}.";
                }
                else if (!workedRecently)
                {
                    reason = $"{user.FullName} ({user.Team} Ekibi • {user.Title}) bu tarihte müsait (Toplam {shiftCount} nöbet görevi var). Son 7 gün içinde nöbeti yok.";
                }
                else
                {
                    reason = $"{user.FullName} ({user.Team} Ekibi) son 7 gün içinde nöbet tuttu. Toplam nöbet sayısı: {shiftCount}.";
                }

                results.Add(new ShiftRecommendationOption
                {
                    CandidateUser = user,
                    ShiftDate = targetDate,
                    HistoricalShiftCount = shiftCount,
                    FairnessScore = displayScore,
                    SortScore = sortScore,
                    IsOnLeaveOnDate = onLeave,
                    IsOnShiftOnDate = onCustomShiftToday,
                    RecommendationReason = reason
                });
            }

            // When "Tüm Ekipler": return top 2 from each team interleaved for balanced results
            bool allTeams = string.IsNullOrWhiteSpace(targetTeamName) || string.Equals(targetTeamName, "Tüm Ekipler", StringComparison.OrdinalIgnoreCase);
            if (allTeams)
            {
                var teamNames = new[] { "Takip", "Tahsis", "Teminat" };
                var interleaved = new List<ShiftRecommendationOption>();

                foreach (var team in teamNames)
                    interleaved.AddRange(results.Where(r => string.Equals(r.CandidateUser.Team, team, StringComparison.OrdinalIgnoreCase))
                                                .OrderByDescending(r => r.SortScore).Take(2));

                var addedIds = interleaved.Select(r => r.CandidateUser.Id).ToHashSet();
                interleaved.AddRange(results.Where(r => !addedIds.Contains(r.CandidateUser.Id))
                                            .OrderByDescending(r => r.SortScore));

                return interleaved;
            }

            return results.OrderByDescending(r => r.SortScore).ToList();
        }
    }
}
