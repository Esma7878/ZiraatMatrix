using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class SmartProjectAllocationTests
    {
        private readonly SmartProjectAllocationService _service;

        public SmartProjectAllocationTests()
        {
            _service = new SmartProjectAllocationService();
        }

        [Fact]
        public void GetDeveloperAllocationRecommendations_ShouldRecommendUserWithZeroActiveProjectsFirst()
        {
            // Arrange
            var userBusy = new User { Id = 1, Name = "Veli", Surname = "Can", Team = "Takip", Title = "Developer" };
            var userFree = new User { Id = 2, Name = "Ayşe", Surname = "Yılmaz", Team = "Takip", Title = "Developer" };

            var allUsers = new List<User> { userBusy, userFree };

            // Veli is assigned to 3 active projects
            var activeProjects = new List<Project>
            {
                new Project { Id = 1, ProjectName = "P1", ProjectStatus = "Devam Ediyor", AssignedDeveloperNames = "Veli Can" },
                new Project { Id = 2, ProjectName = "P2", ProjectStatus = "Planlandı", AssignedDeveloperNames = "Veli Can" },
                new Project { Id = 3, ProjectName = "P3", ProjectStatus = "Devam Ediyor", AssignedDeveloperNames = "Veli Can" }
            };

            // Act
            var recommendations = _service.GetDeveloperAllocationRecommendations(
                requiredTitle: "Developer",
                allUsers: allUsers,
                allProjects: activeProjects,
                teamFilter: "Takip"
            );

            // Assert
            Assert.Equal(2, recommendations.Count);
            
            // Ayşe Yılmaz has 0 projects, so she should be ranked FIRST (lowest workload)
            Assert.Equal("Ayşe Yılmaz", recommendations.First().CandidateUser.FullName);
            Assert.Equal(0, recommendations.First().ActiveProjectCount);
            Assert.Equal(0.0, recommendations.First().WorkloadPercentage);

            // Veli Can should be ranked LAST with 75% workload
            Assert.Equal("Veli Can", recommendations.Last().CandidateUser.FullName);
            Assert.Equal(3, recommendations.Last().ActiveProjectCount);
            Assert.Equal(75.0, recommendations.Last().WorkloadPercentage);
        }
    }
}
