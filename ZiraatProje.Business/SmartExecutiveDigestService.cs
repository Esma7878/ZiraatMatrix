using System;
using System.Collections.Generic;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class ExecutiveDigestReport
    {
        public string Title { get; set; } = "🤖 Ziraat Matrix AI - Günlük Sistem Risk & Durum Özeti";
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public string SummaryText { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new List<string>();
        public List<string> RiskAlerts { get; set; } = new List<string>();
        public int ActiveLeavesCount { get; set; }
        public int TodayShiftsCount { get; set; }
        public int ActiveProjectsCount { get; set; }
    }

    public class SmartExecutiveDigestService
    {
        public ExecutiveDigestReport GenerateDailyDigest(
            IEnumerable<User> allUsers,
            IEnumerable<Leave> allLeaves,
            IEnumerable<Shift> allShifts,
            IEnumerable<Project> allProjects,
            IEnumerable<MonthlyReleaseShift>? monthlyReleases = null,
            IEnumerable<CustomShift>? customShifts = null)
        {
            var report = new ExecutiveDigestReport();
            var today = DateTime.Today;

            var users = allUsers?.ToList() ?? new List<User>();
            var leaves = allLeaves?.Where(l => l.Status == "Approved" || l.Status == "Onaylandı" || string.IsNullOrEmpty(l.Status)).ToList() ?? new List<Leave>();
            var shifts = allShifts?.ToList() ?? new List<Shift>();
            var projects = allProjects?.Where(p => p.ProjectStatus != "Tamamlandı" && p.ProjectStatus != "İptal").ToList() ?? new List<Project>();

            // Today's active leaves
            var todayLeaves = leaves.Where(l => l.StartDate.Date <= today && l.EndDate.Date >= today).ToList();
            report.ActiveLeavesCount = todayLeaves.Count;

            // Gather all personnel names on duty today across all shift tables
            var todayOnDutyNames = new List<string>();

            // 1. Shift table
            foreach (var s in shifts.Where(s => s.ShiftDate.Date == today))
            {
                if (s.User != null && !string.IsNullOrWhiteSpace(s.User.FullName))
                    todayOnDutyNames.Add(s.User.FullName);
            }

            // 2. Monthly / Weekly release shift table
            if (monthlyReleases != null)
            {
                foreach (var m in monthlyReleases.Where(m => !m.IsFinished && m.ReleaseDate.Date == today))
                {
                    if (!string.IsNullOrWhiteSpace(m.AssignedUsers))
                    {
                        var names = m.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim());
                        todayOnDutyNames.AddRange(names);
                    }
                }
            }

            // 3. Custom shift table
            if (customShifts != null)
            {
                foreach (var c in customShifts.Where(c => !c.IsFinished && c.ShiftDate.Date == today))
                {
                    if (!string.IsNullOrWhiteSpace(c.AssignedUsers))
                    {
                        var names = c.AssignedUsers.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim());
                        todayOnDutyNames.AddRange(names);
                    }
                }
            }

            todayOnDutyNames = todayOnDutyNames.Distinct().ToList();
            report.TodayShiftsCount = todayOnDutyNames.Count;
            report.ActiveProjectsCount = projects.Count;

            // 🚨 CRITICAL CONFLICT DETECTION: Check if any person is BOTH on leave AND on duty today!
            var onLeaveUserNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var l in todayLeaves)
            {
                if (l.User != null && !string.IsNullOrWhiteSpace(l.User.FullName))
                {
                    onLeaveUserNames.Add(l.User.FullName.Trim());
                }
            }

            var conflictingPersonnel = todayOnDutyNames
                .Where(dutyName => onLeaveUserNames.Contains(dutyName.Trim()))
                .ToList();

            foreach (var conflictName in conflictingPersonnel)
            {
                report.RiskAlerts.Add($"🚨 **KRİTİK NÖBET-İZİN ÇAKIŞMASI:** '{conflictName}' bugün hem İZİNLİ hem de NÖBETÇİ olarak sistemde kayıtlı! Acilen nöbet devri gerçekleştirilmelidir.");
            }

            // Analyze team capacity risks (%50 same title quota)
            var teamGroups = users.Where(u => !string.IsNullOrWhiteSpace(u.Team)).GroupBy(u => u.Team);
            foreach (var teamGroup in teamGroups)
            {
                var teamMembers = teamGroup.ToList();
                int teamLeavesCount = todayLeaves.Count(l => teamMembers.Any(m => m.Id == l.UserId));
                if (teamMembers.Count > 0)
                {
                    double ratio = ((double)teamLeavesCount / teamMembers.Count) * 100.0;
                    if (ratio >= 50.0)
                    {
                        report.RiskAlerts.Add($"⚠️ **{teamGroup.Key} Ekibi Kotası:** Ekipteki personellerin %{ratio:F0}'i ({teamLeavesCount}/{teamMembers.Count}) bugün izinde!");
                    }
                }
            }

            // Upcoming project deadlines (within 7 days)
            foreach (var p in projects)
            {
                if ((p.EndDate.Date - today).TotalDays <= 7 && (p.EndDate.Date - today).TotalDays >= 0)
                {
                    report.RiskAlerts.Add($"⏳ **Proje Teslim Uyarısı:** '{p.ProjectName}' projesinin son teslim tarihine {(p.EndDate.Date - today).TotalDays:F0} gün kaldı.");
                }
            }

            // Highlights
            if (todayOnDutyNames.Any())
            {
                var shiftStaffNames = string.Join(", ", todayOnDutyNames);
                report.Highlights.Add($"🌙 **Bugünkü Nöbetçiler:** {shiftStaffNames}");
            }
            else
            {
                report.Highlights.Add("ℹ️ Bugün için tanımlı nöbet görevi bulunmuyor.");
            }

            if (todayLeaves.Any())
            {
                var leaveStaffNames = string.Join(", ", todayLeaves.Select(l => l.User?.FullName ?? "Personel").Distinct());
                report.Highlights.Add($"🌴 **Bugün İzindeki Personeller:** {leaveStaffNames}");
            }

            report.Highlights.Add($"🚀 **Aktif Proje Sayısı:** {projects.Count} adet aktif proje yürütülüyor.");

            // Summary text
            if (report.RiskAlerts.Any())
            {
                report.SummaryText = $"⚠️ Sistemde {report.RiskAlerts.Count} adet risk ve çakışma uyarısı tespit edildi! Bugün {report.ActiveLeavesCount} kişi izinde, {report.TodayShiftsCount} nöbetçi görevde.";
            }
            else
            {
                report.SummaryText = $"Sistemde tüm operasyonlar yeşil ve kural ihlali bulunmuyor. Bugün {report.ActiveLeavesCount} kişi izinde, {report.TodayShiftsCount} nöbetçi görevde.";
            }

            return report;
        }
    }
}
