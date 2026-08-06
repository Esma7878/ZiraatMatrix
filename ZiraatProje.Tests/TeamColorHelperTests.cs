using Xunit;
using ZiraatProje.Business;

namespace ZiraatProje.Tests
{
    public class TeamColorHelperTests
    {
        [Theory]
        [InlineData("Takip", "#7c3aed")]
        [InlineData("Tahsis", "#059669")]
        [InlineData("Teminat", "#2563eb")]
        [InlineData("Departman Yönetimi", "#bc171d")]
        [InlineData("Bilinmeyen Ekip", "#7c3aed")]
        public void GetDefaultColorForTeam_ShouldReturnExpectedHexColor(string teamName, string expectedHex)
        {
            // Act
            string hexColor = TeamColorHelper.GetDefaultColorForTeam(teamName);

            // Assert
            Assert.Equal(expectedHex, hexColor);
        }

        [Fact]
        public void RegisterTeamColor_ShouldOverrideDefaultColor()
        {
            // Arrange
            TeamColorHelper.RegisterTeamColor("Özel Ekip", "#ff5722");

            // Act
            string hexColor = TeamColorHelper.GetDefaultColorForTeam("Özel Ekip");

            // Assert
            Assert.Equal("#ff5722", hexColor);
        }

        [Fact]
        public void GetBgColor_ShouldProduceValidHexFormat()
        {
            // Act
            string bgColor = TeamColorHelper.GetBgColor("#7c3aed");

            // Assert
            Assert.StartsWith("#", bgColor);
            Assert.Equal(7, bgColor.Length);
        }
    }
}
