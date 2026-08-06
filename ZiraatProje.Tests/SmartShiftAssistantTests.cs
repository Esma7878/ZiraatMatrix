using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class SmartShiftAssistantTests
    {
        private readonly SmartShiftAssistantService _service;

        public SmartShiftAssistantTests()
        {
            _service = new SmartShiftAssistantService();
        }

        [Fact]
        public void GetShiftRecommendationsForDate_ShouldPenalizeUsersOnLeave()
        {
            // Arrange
            var targetDate = new DateTime(2026, 9, 15);

            var userAvailable = new User { Id = 1, Name = "Serkan", Surname = "Vural", Team = "Takip", Title = "Yazılımcı" };
            var userOnLeave = new User { Id = 2, Name = "Gökhan", Surname = "Kurt", Team = "Takip", Title = "Yazılımcı" };

            var allUsers = new List<User> { userAvailable, userOnLeave };

            var allLeaves = new List<Leave>
            {
                new Leave
                {
                    UserId = 2,
                    StartDate = targetDate.AddDays(-1),
                    EndDate = targetDate.AddDays(1),
                    Status = "Approved",
                    User = userOnLeave
                }
            };

            // Act
            var recommendations = _service.GetShiftRecommendationsForDate(
                targetDate: targetDate,
                targetTeamName: "Takip",
                targetShiftType: "Server Geçiş Nöbeti",
                allUsers: allUsers,
                allShifts: new List<Shift>(),
                allLeaves: allLeaves
            );

            // Assert
            Assert.Equal(2, recommendations.Count);
            
            var topRecommendation = recommendations.First();
            Assert.Equal("Serkan Vural", topRecommendation.CandidateUser.FullName);
            Assert.False(topRecommendation.IsOnLeaveOnDate);

            var bottomRecommendation = recommendations.Last();
            Assert.Equal("Gökhan Kurt", bottomRecommendation.CandidateUser.FullName);
            Assert.True(bottomRecommendation.IsOnLeaveOnDate);
            Assert.Contains("İZİNLİ olduğu için önerilmiyor", bottomRecommendation.RecommendationReason);
        }

        [Fact]
        public void GetShiftRecommendationsForDate_ShouldInterleaveTopCandidatesPerTeam_WhenAllTeamsSelected()
        {
            // Arrange
            var targetDate = DateTime.Today;

            var uTakip = new User { Id = 1, Name = "TakipUser", Team = "Takip", Title = "Dev" };
            var uTahsis = new User { Id = 2, Name = "TahsisUser", Team = "Tahsis", Title = "Dev" };
            var uTeminat = new User { Id = 3, Name = "TeminatUser", Team = "Teminat", Title = "Dev" };

            var allUsers = new List<User> { uTakip, uTahsis, uTeminat };

            // Act
            var recommendations = _service.GetShiftRecommendationsForDate(
                targetDate: targetDate,
                targetTeamName: "Tüm Ekipler",
                targetShiftType: "Genel Nöbet",
                allUsers: allUsers,
                allShifts: new List<Shift>(),
                allLeaves: new List<Leave>()
            );

            // Assert
            Assert.Equal(3, recommendations.Count);
            var teamsInResult = recommendations.Select(r => r.CandidateUser.Team).Distinct().ToList();
            Assert.Contains("Takip", teamsInResult);
            Assert.Contains("Tahsis", teamsInResult);
            Assert.Contains("Teminat", teamsInResult);
        }
    }
}
