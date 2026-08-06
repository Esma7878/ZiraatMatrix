using System;
using Xunit;
using ZiraatProje.Business;

namespace ZiraatProje.Tests
{
    public class ValidationRulesTests
    {
        [Fact]
        public void ValidateLeaveDates_ShouldThrowException_WhenStartDateIsInThePast()
        {
            // Arrange
            DateTime pastStartDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                ValidationRules.ValidateLeaveDates(pastStartDate, endDate));

            Assert.Contains("Geçmiş tarihli izin girişi yapılamaz", exception.Message);
        }

        [Fact]
        public void ValidateLeaveDates_ShouldThrowException_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(5);
            DateTime invalidEndDate = DateTime.Today.AddDays(3);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                ValidationRules.ValidateLeaveDates(startDate, invalidEndDate));

            Assert.Contains("başlangıç tarihinden önce olamaz", exception.Message);
        }

        [Fact]
        public void ValidateLeaveDates_ShouldNotThrow_WhenDatesAreValid()
        {
            // Arrange
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(3);

            // Act & Assert (No exception expected)
            ValidationRules.ValidateLeaveDates(startDate, endDate);
        }
    }
}
