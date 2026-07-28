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
            string targetShiftType,
            IEnumerable<User> allUsers,
            IEnumerable<Shift> allShifts,
            IEnumerable<Leave> allLeaves,
            IEnumerable<CustomShift>? allCustomShifts = null,
            IEnumerable<MonthlyReleaseShift>? allMonthlyReleaseShifts = null)
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
            var releaseShiftList = allMonthlyReleaseShifts?.ToList() ?? new List<MonthlyReleaseShift>();

            bool isSpecificShiftType = !string.IsNullOrWhiteSpace(targetShiftType) && !string.Equals(targetShiftType, "Tüm Nöbet Türleri", StringComparison.OrdinalIgnoreCase);

            foreach (var user in candidateUsers)
            {
                // 1. Calculate overall total shifts (all categories)
                int regularShiftCount = existingShifts.Count(s => s.UserId == user.Id);
                int customShiftCount = customShiftList.Count(cs =>
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));
                int releaseShiftCount = releaseShiftList.Count(rs =>
                    !string.IsNullOrWhiteSpace(rs.AssignedUsers) &&
                    rs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                int totalShiftCount = regularShiftCount + customShiftCount + releaseShiftCount;

                // 2. Calculate TYPE-SPECIFIC shift history for the requested shift type
                int typeShiftCount = 0;
                DateTime? lastTypeShiftDate = null;

                if (isSpecificShiftType)
                {
                    if (string.Equals(targetShiftType, "Haftalık Nöbet", StringComparison.OrdinalIgnoreCase))
                    {
                        var userWeeklyShifts = releaseShiftList
                            .Where(rs => !string.IsNullOrWhiteSpace(rs.MonthName) &&
                                         rs.MonthName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase) &&
                                         !string.IsNullOrWhiteSpace(rs.AssignedUsers) &&
                                         rs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(rs => rs.ReleaseDate)
                            .ToList();

                        typeShiftCount = userWeeklyShifts.Count;
                        if (userWeeklyShifts.Any()) lastTypeShiftDate = userWeeklyShifts.First().ReleaseDate;
                    }
                    else if (string.Equals(targetShiftType, "Yaygınlaştırma Nöbeti", StringComparison.OrdinalIgnoreCase) ||
                             targetShiftType.Contains("Yaygınlaştırma", StringComparison.OrdinalIgnoreCase))
                    {
                        var userReleaseShifts = releaseShiftList
                            .Where(rs => (string.IsNullOrWhiteSpace(rs.MonthName) || !rs.MonthName.Contains("Haftalık", StringComparison.OrdinalIgnoreCase)) &&
                                         !string.IsNullOrWhiteSpace(rs.AssignedUsers) &&
                                         rs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(rs => rs.ReleaseDate)
                            .ToList();

                        typeShiftCount = userReleaseShifts.Count;
                        if (userReleaseShifts.Any()) lastTypeShiftDate = userReleaseShifts.First().ReleaseDate;
                    }
                    else
                    {
                        // Custom shift type (e.g. Yılbaşı Nöbeti, Server Geçiş Nöbeti, Firewall Geçiş Nöbeti, etc.)
                        var userCustomTypeShifts = customShiftList
                            .Where(cs => !string.IsNullOrWhiteSpace(cs.Topic) &&
                                         (cs.Topic.Contains(targetShiftType, StringComparison.OrdinalIgnoreCase) || targetShiftType.Contains(cs.Topic, StringComparison.OrdinalIgnoreCase)) &&
                                         !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                                         cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(cs => cs.ShiftDate)
                            .ToList();

                        typeShiftCount = userCustomTypeShifts.Count;
                        if (userCustomTypeShifts.Any()) lastTypeShiftDate = userCustomTypeShifts.First().ShiftDate;
                    }
                }

                // 3. Leave check on target date
                bool onLeave = activeLeaves.Any(l =>
                    (l.UserId == user.Id || (l.User != null && string.Equals(l.User.FullName, user.FullName, StringComparison.OrdinalIgnoreCase))) &&
                    l.StartDate.Date <= targetDate.Date && l.EndDate.Date >= targetDate.Date);

                // 4. Shift check on target date
                bool onShiftToday = customShiftList.Any(cs =>
                    cs.ShiftDate.Date == targetDate.Date &&
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase))
                    || releaseShiftList.Any(rs =>
                    rs.ReleaseDate.Date == targetDate.Date &&
                    !string.IsNullOrWhiteSpace(rs.AssignedUsers) &&
                    rs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                // 5. Worked recently within last 7 days?
                bool workedRecently = customShiftList.Any(cs =>
                    Math.Abs((cs.ShiftDate.Date - targetDate.Date).TotalDays) <= 7 &&
                    !string.IsNullOrWhiteSpace(cs.AssignedUsers) &&
                    cs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase))
                    || releaseShiftList.Any(rs =>
                    Math.Abs((rs.ReleaseDate.Date - targetDate.Date).TotalDays) <= 7 &&
                    !string.IsNullOrWhiteSpace(rs.AssignedUsers) &&
                    rs.AssignedUsers.Contains(user.FullName, StringComparison.OrdinalIgnoreCase));

                // 6. Calculate Fairness & Sort Score
                double baseScore = 95.0 - (totalShiftCount * 5.0);

                double daysSinceLastType = 9999;
                if (isSpecificShiftType && lastTypeShiftDate.HasValue)
                {
                    daysSinceLastType = (targetDate.Date - lastTypeShiftDate.Value.Date).TotalDays;
                }

                if (isSpecificShiftType)
                {
                    if (typeShiftCount == 0)
                    {
                        baseScore += 10.0;
                    }
                    else
                    {
                        // Score boost proportional to how long ago they worked this specific shift type (years/months ago)
                        if (daysSinceLastType > 365)
                        {
                            double yearsAgo = daysSinceLastType / 365.0;
                            baseScore += Math.Min(15.0, yearsAgo * 3.0);
                        }
                        else
                        {
                            baseScore -= Math.Max(20.0, 50.0 - (daysSinceLastType / 10.0));
                        }
                    }
                }

                if (workedRecently) baseScore -= 20.0;
                if (onLeave) baseScore -= 60.0;
                if (onShiftToday) baseScore -= 80.0;

                double displayScore = Math.Min(100.0, Math.Max(5.0, baseScore));
                double sortScore = displayScore - (((user.Id * 7 + targetDate.DayOfYear * 13) % 11) * 0.05);

                // 7. Construct Rich Human-Readable Recommendation Reason
                string scoreStr = $"%{Math.Min(100.0, Math.Max(0.0, displayScore)):F0}";
                string reason;
                if (onLeave)
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi) {targetDate:dd.MM.yyyy} tarihinde İZİNLİ olduğu için önerilmiyor.";
                }
                else if (onShiftToday)
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi • {user.Title}) {targetDate:dd.MM.yyyy} tarihinde ZATEN NÖBETÇİ. Toplam nöbet sayısı: {totalShiftCount}.";
                }
                else if (isSpecificShiftType)
                {
                    if (typeShiftCount == 0)
                    {
                        if (totalShiftCount <= 2)
                        {
                            reason = $"💡 {user.FullName} ({user.Team} Ekibi • {user.Title}): Daha önce hiç '{targetShiftType}' tutmamıştır (0 nöbet) VE genel nöbet yükü çok düşüktür (Genel toplam {totalShiftCount} nöbet). Bu sebeple Adillik Puanı {scoreStr} hesaplanmıştır.";
                        }
                        else
                        {
                            reason = $"💡 {user.FullName} ({user.Team} Ekibi • {user.Title}): Daha önce hiç '{targetShiftType}' tutmamıştır (0 nöbet). Ancak diğer türlerdeki genel nöbet yükü yüksek olduğu için (Genel toplam {totalShiftCount} nöbet) Adillik Puanı {scoreStr} hesaplanmıştır.";
                        }
                    }
                    else
                    {
                        string lastDateText = lastTypeShiftDate.HasValue ? $"{lastTypeShiftDate.Value:dd.MM.yyyy}" : "geçmişte";
                        double yearsAgo = Math.Round(daysSinceLastType / 365.0, 1);
                        
                        if (daysSinceLastType > 365)
                        {
                            reason = $"💡 {user.FullName} ({user.Team} Ekibi • {user.Title}): '{targetShiftType}' türünde en son {lastDateText} tarihinde ({yearsAgo} yıl önce) görev almıştır. Uzun süredir bu türde nöbet tutmadığı için Adillik Puanı {scoreStr} hesaplanmıştır.";
                        }
                        else
                        {
                            reason = $"💡 {user.FullName} ({user.Team} Ekibi • {user.Title}): '{targetShiftType}' türünde yakın bir tarihte ({lastDateText}) görev almıştır (Bu türde {typeShiftCount} nöbet, Genel toplam {totalShiftCount} nöbet). Rotasyon gereği Adillik Puanı {scoreStr} hesaplanmıştır.";
                        }
                    }
                }
                else if (!workedRecently)
                {
                    reason = $"💡 {user.FullName} ({user.Team} Ekibi • {user.Title}): Bu tarihte tamamen müsaittir (Genel toplam {totalShiftCount} nöbet görevi var). Son 7 gün içinde nöbeti olmadığı için Adillik Puanı {scoreStr} hesaplanmıştır.";
                }
                else
                {
                    reason = $"⚠️ {user.FullName} ({user.Team} Ekibi): Son 7 gün içinde nöbet tutmuştur (Genel toplam {totalShiftCount} nöbet). Dinlenme süresi nedeniyle Adillik Puanı {scoreStr} hesaplanmıştır.";
                }

                results.Add(new ShiftRecommendationOption
                {
                    CandidateUser = user,
                    ShiftDate = targetDate,
                    ShiftTypeName = isSpecificShiftType ? targetShiftType : "Genel Nöbet",
                    HistoricalShiftCount = totalShiftCount,
                    FairnessScore = displayScore,
                    SortScore = sortScore,
                    IsOnLeaveOnDate = onLeave,
                    IsOnShiftOnDate = onShiftToday,
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
