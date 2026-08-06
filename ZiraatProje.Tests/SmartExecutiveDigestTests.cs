using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class SmartExecutiveDigestTests
    {
        private readonly SmartExecutiveDigestService _service;

        public SmartExecutiveDigestTests()
        {
            _service = new SmartExecutiveDigestService();
        }

        [Fact]
        public void GenerateDailyDigest_ShouldAccuratelyCalculateActiveLeavesAndProjectsCount()
        {
            // Arrange
            var today = DateTime.Today;

            var user1 = new User { Id = 1, Name = "Selin", Surname = "Arslan", Team = "Takip", Title = "Yazılımcı" };
            var user2 = new User { Id = 2, Name = "Burak", Surname = "Demir", Team = "Tahsis", Title = "Analist" };
            var allUsers = new List<User> { user1, user2 };

            // User1 is currently on leave today
            var allLeaves = new List<Leave>
            {
                new Leave
                {
                    UserId = 1,
                    StartDate = today.AddDays(-1),
                    EndDate = today.AddDays(2),
                    Status = "Approved",
                    User = user1
                }
            };

            var allShifts = new List<Shift>();

            var allProjects = new List<Project>
            {
                new Project { Id = 1, ProjectName = "Mobil Onay", ProjectStatus = "Devam Ediyor" },
                new Project { Id = 2, ProjectName = "Kredi Karar Sistemi", ProjectStatus = "Planlandı" },
                new Project { Id = 3, ProjectName = "Eski Raporlama", ProjectStatus = "Tamamlandı" } // Should be excluded from active count
            };

            // Act
            var report = _service.GenerateDailyDigest(
                allUsers: allUsers,
                allLeaves: allLeaves,
                allShifts: allShifts,
                allProjects: allProjects
            );

            // Assert
            Assert.NotNull(report);
            Assert.Equal(1, report.ActiveLeavesCount);
            Assert.Equal(2, report.ActiveProjectsCount); // "Devam Ediyor" & "Planlandı"
            Assert.NotEmpty(report.SummaryText);
        }

        [Fact]
        public void GenerateDailyDigest_ShouldGenerateRiskAlert_WhenPersonnelOnLeaveHasShiftToday()
        {
            // Arrange
            var today = DateTime.Today;
            var user = new User { Id = 1, Name = "Can", Surname = "Öztürk", Team = "Takip", Title = "Analist" };
            var allUsers = new List<User> { user };

            var allLeaves = new List<Leave>
            {
                new Leave
                {
                    UserId = 1,
                    StartDate = today,
                    EndDate = today,
                    Status = "Approved",
                    User = user
                }
            };

            var allShifts = new List<Shift>
            {
                new Shift
                {
                    UserId = 1,
                    ShiftDate = today,
                    User = user,
                    ShiftType = new ShiftType { ShiftName = "Server Geçiş Nöbeti" }
                }
            };

            // Act
            var report = _service.GenerateDailyDigest(
                allUsers: allUsers,
                allLeaves: allLeaves,
                allShifts: allShifts,
                allProjects: new List<Project>()
            );

            // Assert
            Assert.NotEmpty(report.RiskAlerts);
            Assert.Contains(report.RiskAlerts, alert => alert.Contains("hem İZİNLİ hem de NÖBETÇİ"));
        }
    }
}
