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
        public string RecommendationReason { get; set; } = string.Empty;

        public string TeamBadgeText => string.IsNullOrWhiteSpace(CandidateUser.Team) ? CandidateUser.Title : $"{CandidateUser.Team} Ekibi • {CandidateUser.Title}";
        public string BadgeText => $"🏆 Adillik Puanı: %{Math.Min(100.0, Math.Max(0.0, FairnessScore)):F0}";
        public string StatusBadge => IsOnLeaveOnDate ? "⚠️ Personel O Tarihte İzinde!" : "✅ Nöbet ve İzin Durumu Uygun";
    }

    public class SmartShiftAssistantService
    {
        public List<ShiftRecommendationOption> GetShiftRecommendationsForDate(
            DateTime targetDate,
            string targetTeamName,
            IEnumerable<User> allUsers,
            IEnumerable<Shift> allShifts,
            IEnumerable<Leave> allLeaves)
        {
            var results = new List<ShiftRecommendationOption>();
            if (allUsers == null) return results;

            // Filter users by team if specified (and not "Tüm Ekipler")
            var candidateUsers = allUsers.Where(u => !u.IsAdmin).ToList();

            if (!string.IsNullOrWhiteSpace(targetTeamName) && !string.Equals(targetTeamName, "Tüm Ekipler", StringComparison.OrdinalIgnoreCase))
            {
                var teamFiltered = candidateUsers.Where(u => string.Equals(u.Team, targetTeamName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (teamFiltered.Any())
                {
                    candidateUsers = teamFiltered;
                }
            }

            var activeLeaves = allLeaves?.Where(l => l.Status != "Rejected" && l.Status != "Reddedildi").ToList() ?? new List<Leave>();
            var existingShifts = allShifts?.ToList() ?? new List<Shift>();

            foreach (var user in candidateUsers)
            {
                // Total shift count for user
                int shiftCount = existingShifts.Count(s => s.UserId == user.Id);

                // Is user on leave on target date?
                bool onLeave = activeLeaves.Any(l => (l.UserId == user.Id || (l.User != null && string.Equals(l.User.FullName, user.FullName, StringComparison.OrdinalIgnoreCase))) && l.StartDate.Date <= targetDate.Date && l.EndDate.Date >= targetDate.Date);

                // Has user worked a shift in the last 7 days?
                bool workedRecently = existingShifts.Any(s => s.UserId == user.Id && Math.Abs((s.ShiftDate.Date - targetDate.Date).TotalDays) <= 7);

                // Base fairness score (starts at 100 max)
                double baseScore = 100.0 - (shiftCount * 12.0);
                if (workedRecently) baseScore -= 25.0;
                if (onLeave) baseScore -= 60.0;

                // Clean percentage display (%100 for completely free personnel)
                double displayScore = Math.Min(100.0, Math.Max(5.0, baseScore));

                // Micro tie-breaker sorting score (doesn't alter displayScore percentage!)
                double sortScore = displayScore - (((user.Id * 7 + targetDate.DayOfYear * 13) % 11) * 0.05);

                string reason;
                if (!onLeave && !workedRecently)
                {
                    reason = $"{user.FullName} ({user.Team} Ekibi • {user.Title}) bu tarihte müsait (Toplam {shiftCount} nöbet görevi var). Son 7 gün içinde nöbeti yok.";
                }
                else if (onLeave)
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi) bu tarihte izinli olduğu için önerilmiyor.";
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
                    RecommendationReason = reason
                });
            }

            return results.OrderByDescending(r => r.SortScore).ToList();
        }
    }
}
