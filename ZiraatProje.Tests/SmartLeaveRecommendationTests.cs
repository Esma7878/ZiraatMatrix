using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZiraatProje.Business;
using ZiraatProje.DataAccess;

namespace ZiraatProje.Tests
{
    public class SmartLeaveRecommendationTests
    {
        private readonly SmartLeaveRecommendationService _service;

        public SmartLeaveRecommendationTests()
        {
            _service = new SmartLeaveRecommendationService();
        }

        [Fact]
        public void GetRecommendations_ShouldDetectSameTitleConflict_WhenTeammatesAreOnLeaveEntireMonth()
        {
            // Arrange
            var currentUser = new User { Id = 1, Name = "Ahmet", Team = "Takip", Title = "Developer" };
            var teammate = new User { Id = 2, Name = "Mehmet", Team = "Takip", Title = "Developer" };

            var allUsers = new List<User> { currentUser, teammate };

            // Mehmet is on approved leave for the ENTIRE month of August 2026
            var allLeaves = new List<Leave>
            {
                new Leave
                {
                    Id = 10,
                    UserId = 2,
                    StartDate = new DateTime(2026, 8, 1),
                    EndDate = new DateTime(2026, 8, 31),
                    Status = "Approved"
                }
            };

            var allShifts = new List<Shift>();

            // Act
            var recommendations = _service.GetRecommendations(
                currentUser: currentUser,
                desiredWorkingDays: 5,
                targetYear: 2026,
                targetMonth: 8,
                allUsers: allUsers,
                allLeaves: allLeaves,
                allShifts: allShifts
            );

            // Assert
            Assert.NotEmpty(recommendations);
            var topOption = recommendations.First();

            // All options in August will suffer title conflict (100% ratio)
            Assert.True(topOption.SameTitleLeaveRatio > 50.0);
            Assert.Contains("Unvan Kota Uyumsuz", topOption.CapacityBadge);
        }

        [Fact]
        public void GetRecommendations_ShouldDetectShiftConflict_WhenUserHasShiftsAcrossMonth()
        {
            // Arrange
            var currentUser = new User { Id = 1, Name = "Ali", Team = "Tahsis", Title = "Analist" };
            var allUsers = new List<User> { currentUser };
            var allLeaves = new List<Leave>();

            // Ali has shifts scheduled throughout August 2026
            var allShifts = new List<Shift>();
            for (int day = 1; day <= 31; day++)
            {
                allShifts.Add(new Shift
                {
                    Id = day,
                    UserId = 1,
                    ShiftDate = new DateTime(2026, 8, day),
                    ShiftTypeId = 1
                });
            }

            // Act
            var recommendations = _service.GetRecommendations(
                currentUser: currentUser,
                desiredWorkingDays: 5,
                targetYear: 2026,
                targetMonth: 8,
                allUsers: allUsers,
                allLeaves: allLeaves,
                allShifts: allShifts
            );

            // Assert
            Assert.NotEmpty(recommendations);
            var shiftConflictOption = recommendations.FirstOrDefault(r => r.HasShiftConflict);
            Assert.NotNull(shiftConflictOption);
            Assert.Contains("Nöbet Çakışması Var", shiftConflictOption.ShiftBadge);
        }
    }
}
