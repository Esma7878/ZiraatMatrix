using System;
using System.Collections.Generic;
using System.Linq;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Business
{
    public class ProjectAllocationRecommendation
    {
        public User CandidateUser { get; set; } = new User();
        public int ActiveProjectCount { get; set; }
        public double WorkloadPercentage { get; set; }
        public string RecommendationReason { get; set; } = string.Empty;

        public string TeamBadgeText => string.IsNullOrWhiteSpace(CandidateUser.Team) ? "Departman" : $"{CandidateUser.Team} Ekibi";
        public string WorkloadBadge => $"📊 Doluluk: %{WorkloadPercentage:F0}";
        public string StatusBadge => WorkloadPercentage > 75 ? "⚠️ Yüksek İş Yükü" : "✅ Atama İçin Uygun";
    }

    public class SmartProjectAllocationService
    {
        public List<ProjectAllocationRecommendation> GetDeveloperAllocationRecommendations(
            string requiredTitle,
            IEnumerable<User> allUsers,
            IEnumerable<Project> allProjects)
        {
            var results = new List<ProjectAllocationRecommendation>();
            if (allUsers == null) return results;

            var activeProjects = allProjects?.Where(p => p.ProjectStatus != "Tamamlandı" && p.ProjectStatus != "İptal").ToList() ?? new List<Project>();

            var candidateUsers = allUsers.Where(u => !u.IsAdmin).ToList();
            if (!string.IsNullOrWhiteSpace(requiredTitle))
            {
                var titleFiltered = candidateUsers.Where(u => string.Equals(u.Title, requiredTitle, StringComparison.OrdinalIgnoreCase)).ToList();
                if (titleFiltered.Any()) candidateUsers = titleFiltered;
            }

            foreach (var user in candidateUsers)
            {
                int assignedCount = activeProjects.Count(p =>
                    (!string.IsNullOrWhiteSpace(p.AssignedDeveloperNames) && p.AssignedDeveloperNames.Contains(user.FullName, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.AssignedUserNames) && p.AssignedUserNames.Contains(user.FullName, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.AssignedUserIds) && p.AssignedUserIds.Split(',').Contains(user.Id.ToString()))
                );

                double workload = Math.Min(100.0, assignedCount * 25.0);

                string teamName = string.IsNullOrWhiteSpace(user.Team) ? "Departman" : $"{user.Team} Ekibi";

                string reason;
                if (assignedCount == 0)
                {
                    reason = $"{user.FullName} ({teamName} • {user.Title}) şu anda aktif projede yer almıyor. Yeni proje için en uygun aday.";
                }
                else if (assignedCount <= 2)
                {
                    reason = $"{user.FullName} ({teamName} • {user.Title}) {assignedCount} aktif projede görevli (%{workload:F0} doluluk). Atama yapılabilir.";
                }
                else
                {
                    reason = $"⚠️ {user.FullName} ({teamName} • {user.Title}) {assignedCount} aktif projede görevli (%{workload:F0} doluluk). İş yükü yüksek.";
                }

                results.Add(new ProjectAllocationRecommendation
                {
                    CandidateUser = user,
                    ActiveProjectCount = assignedCount,
                    WorkloadPercentage = workload,
                    RecommendationReason = reason
                });
            }

            return results.OrderBy(r => r.WorkloadPercentage).ThenBy(r => r.ActiveProjectCount).ToList();
        }
    }
}
