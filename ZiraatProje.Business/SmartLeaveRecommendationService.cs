using System;
using System.Collections.Generic;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class LeaveRecommendationOption
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RequiredLeaveDays { get; set; }
        public int TotalConsecutiveRestDays { get; set; }
        public double SameTitleLeaveRatio { get; set; }
        public bool HasShiftConflict { get; set; }
        public double MatchScore { get; set; }
        public string RecommendationReason { get; set; } = string.Empty;

        public string BadgeText => $"🌟 {TotalConsecutiveRestDays} Gün Tatil / {RequiredLeaveDays} Gün İzin";
        public string CapacityBadge => SameTitleLeaveRatio > 50
            ? $"⚠️ Unvan Kota Uyumsuz (%{SameTitleLeaveRatio:F0})"
            : $"✅ Unvan Kotası Uyumlu (%{SameTitleLeaveRatio:F0})";
        public string ShiftBadge => HasShiftConflict ? "⚠️ Nöbet Çakışması Var" : "✅ Nöbet Çakışması Yok";
        public string FormattedRange => StartDate.Date == EndDate.Date ? $"{StartDate:dd MMMM yyyy, dddd}" : $"{StartDate:dd MMMM (dddd)} - {EndDate:dd MMMM yyyy (dddd)}";
    }

    public class SmartLeaveRecommendationService
    {
        public List<LeaveRecommendationOption> GetRecommendations(
            User currentUser,
            int desiredWorkingDays,
            int targetYear,
            int targetMonth,
            IEnumerable<User> allUsers,
            IEnumerable<Leave> allLeaves,
            IEnumerable<Shift> allShifts)
        {
            var results = new List<LeaveRecommendationOption>();
            if (currentUser == null || desiredWorkingDays <= 0) return results;

            int daysInMonth = DateTime.DaysInMonth(targetYear, targetMonth);
            var monthStart = new DateTime(targetYear, targetMonth, 1);
            var monthEnd = new DateTime(targetYear, targetMonth, daysInMonth);

            var teammatesSameTitle = allUsers.Where(u =>
                u.Id != currentUser.Id &&
                !string.IsNullOrWhiteSpace(u.Team) &&
                !string.IsNullOrWhiteSpace(u.Title) &&
                string.Equals(u.Team, currentUser.Team, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Title, currentUser.Title, StringComparison.OrdinalIgnoreCase)).ToList();

            int sameTitleGroupTotal = teammatesSameTitle.Count + 1;

            var activeLeaves = allLeaves.Where(l =>
                l.Status != "Rejected" &&
                l.Status != "Reddedildi" &&
                l.StartDate <= monthEnd.AddDays(15) &&
                l.EndDate >= monthStart.AddDays(-15)).ToList();

            var userShifts = allShifts.Where(s => s.UserId == currentUser.Id).Select(s => s.ShiftDate.Date).ToHashSet();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var candidateStart = new DateTime(targetYear, targetMonth, day);

                // Leave Start MUST be an actual working day (cannot be Saturday, Sunday, or Official Holiday)
                if (candidateStart.DayOfWeek == DayOfWeek.Saturday || candidateStart.DayOfWeek == DayOfWeek.Sunday || IsOfficialHoliday(candidateStart))
                {
                    continue;
                }

                int accumulatedWorkingDays = 0;
                var currentDay = candidateStart;
                while (accumulatedWorkingDays < desiredWorkingDays)
                {
                    bool isWeekend = currentDay.DayOfWeek == DayOfWeek.Saturday || currentDay.DayOfWeek == DayOfWeek.Sunday;
                    bool isHoliday = IsOfficialHoliday(currentDay);

                    if (!isWeekend && !isHoliday)
                    {
                        accumulatedWorkingDays++;
                    }

                    if (accumulatedWorkingDays < desiredWorkingDays)
                    {
                        currentDay = currentDay.AddDays(1);
                    }
                }
                var candidateEnd = currentDay;

                // Expand restStart backwards for preceding weekend/holiday
                var restStart = candidateStart;
                while (restStart.AddDays(-1).DayOfWeek == DayOfWeek.Saturday || 
                       restStart.AddDays(-1).DayOfWeek == DayOfWeek.Sunday || 
                       IsOfficialHoliday(restStart.AddDays(-1)))
                {
                    restStart = restStart.AddDays(-1);
                }

                // Expand restEnd forwards for succeeding weekend/holiday
                var restEnd = candidateEnd;
                while (restEnd.AddDays(1).DayOfWeek == DayOfWeek.Saturday || 
                       restEnd.AddDays(1).DayOfWeek == DayOfWeek.Sunday || 
                       IsOfficialHoliday(restEnd.AddDays(1)))
                {
                    restEnd = restEnd.AddDays(1);
                }

                int totalRestDays = (int)(restEnd.Date - restStart.Date).TotalDays + 1;

                double maxSameTitleRatio = 0;
                bool shiftConflict = false;

                for (var d = candidateStart.Date; d <= candidateEnd.Date; d = d.AddDays(1))
                {
                    if (userShifts.Contains(d))
                    {
                        shiftConflict = true;
                    }

                    int teammatesOnLeave = teammatesSameTitle.Count(t => activeLeaves.Any(l => l.UserId == t.Id && l.StartDate.Date <= d && l.EndDate.Date >= d));
                    double ratio = ((double)(teammatesOnLeave + 1) / sameTitleGroupTotal) * 100.0;
                    if (ratio > maxSameTitleRatio)
                    {
                        maxSameTitleRatio = ratio;
                    }
                }

                double score = 100.0;
                double restEfficiency = (double)totalRestDays / desiredWorkingDays;
                score += restEfficiency * 10.0;

                if (maxSameTitleRatio > 50.0)
                {
                    score -= (maxSameTitleRatio - 50.0) * 2.5;
                }

                if (shiftConflict)
                {
                    score -= 25.0;
                }

                string reason;
                if (maxSameTitleRatio <= 50.0 && !shiftConflict)
                {
                    reason = $"{desiredWorkingDays} çalışma günü izin ({candidateStart:dd MMMM dddd} - {candidateEnd:dd MMMM dddd}) harcayarak {totalRestDays} gün ({restStart:dd MMMM dddd} - {restEnd:dd MMMM dddd}) kesintisiz tatil imkanı. Ekip içi unvan izin kotası (%{maxSameTitleRatio:F0}) %50 sınırının altında ve nöbet çakışmanız bulunmuyor.";
                }
                else if (shiftConflict)
                {
                    reason = $"{desiredWorkingDays} gün izin ile {totalRestDays} gün tatil ({restStart:dd MMMM} - {restEnd:dd MMMM}). Dikkat: Bu tarihlerde nöbet göreviniz bulunmaktadır.";
                }
                else
                {
                    reason = $"{desiredWorkingDays} gün izin ile {totalRestDays} gün tatil ({restStart:dd MMMM} - {restEnd:dd MMMM}). Dikkat: Aynı unvandaki çalışanların %{maxSameTitleRatio:F0}'i izinde olacağı için %50 kuralı aşılıyor.";
                }

                results.Add(new LeaveRecommendationOption
                {
                    StartDate = candidateStart,
                    EndDate = candidateEnd,
                    RequiredLeaveDays = desiredWorkingDays,
                    TotalConsecutiveRestDays = totalRestDays,
                    SameTitleLeaveRatio = maxSameTitleRatio,
                    HasShiftConflict = shiftConflict,
                    MatchScore = score,
                    RecommendationReason = reason
                });
            }

            return results
                .OrderByDescending(r => r.MatchScore)
                .ThenByDescending(r => r.TotalConsecutiveRestDays)
                .GroupBy(r => $"{r.StartDate:yyyyMMdd}_{r.EndDate:yyyyMMdd}")
                .Select(g => g.First())
                .Take(3)
                .ToList();
        }

        private bool IsOfficialHoliday(DateTime date)
        {
            int m = date.Month;
            int d = date.Day;
            if ((m == 1 && d == 1) || (m == 4 && d == 23) || (m == 5 && d == 1) ||
                (m == 5 && d == 19) || (m == 7 && d == 15) || (m == 8 && d == 30) || (m == 10 && d == 29))
            {
                return true;
            }
            return false;
        }
    }
}
