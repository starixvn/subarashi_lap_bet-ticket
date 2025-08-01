using Fanex.Athena.BetTicket.Result.MetaModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.MetaModels
{
    public class SportsResultMetaModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var metaModel = new SportsResultMetaModel();

            // Assert
            Assert.NotNull(metaModel);
        }

        [Fact]
        public void MatchId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();
            var expectedMatchId = 123456789;

            // Act
            metaModel.MatchId = expectedMatchId;

            // Assert
            Assert.Equal(expectedMatchId, metaModel.MatchId);
        }

        [Fact]
        public void Bettype_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();
            var expectedBettype = 1;

            // Act
            metaModel.Bettype = expectedBettype;

            // Assert
            Assert.Equal(expectedBettype, metaModel.Bettype);
        }

        [Fact]
        public void Language_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();
            var expectedLanguage = "en";

            // Act
            metaModel.Language = expectedLanguage;

            // Assert
            Assert.Equal(expectedLanguage, metaModel.Language);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void MatchId_WithVariousValues_ShouldBeSet(int matchId)
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();

            // Act
            metaModel.MatchId = matchId;

            // Assert
            Assert.Equal(matchId, metaModel.MatchId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Bettype_WithVariousValues_ShouldBeSet(int bettype)
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();

            // Act
            metaModel.Bettype = bettype;

            // Assert
            Assert.Equal(bettype, metaModel.Bettype);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("en")]
        [InlineData("th")]
        [InlineData("zh-CN")]
        [InlineData("fr-FR")]
        public void Language_WithVariousValues_ShouldBeSet(string language)
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();

            // Act
            metaModel.Language = language;

            // Assert
            Assert.Equal(language, metaModel.Language);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();
            var expectedMatchId = 123456789;
            var expectedBettype = 1;
            var expectedLanguage = "en";

            // Act
            metaModel.MatchId = expectedMatchId;
            metaModel.Bettype = expectedBettype;
            metaModel.Language = expectedLanguage;

            // Assert
            Assert.Equal(expectedMatchId, metaModel.MatchId);
            Assert.Equal(expectedBettype, metaModel.Bettype);
            Assert.Equal(expectedLanguage, metaModel.Language);
        }

        [Fact]
        public void DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var metaModel = new SportsResultMetaModel();

            // Assert
            Assert.Equal(0, metaModel.MatchId); // Default int value
            Assert.Equal(0, metaModel.Bettype); // Default int value
            Assert.Null(metaModel.Language); // Default string value
        }

        [Fact]
        public void MultipleInstances_ShouldHaveIndependentValues()
        {
            // Arrange
            var metaModel1 = new SportsResultMetaModel();
            var metaModel2 = new SportsResultMetaModel();

            // Act
            metaModel1.MatchId = 123456789;
            metaModel1.Bettype = 1;
            metaModel1.Language = "en";

            metaModel2.MatchId = 987654321;
            metaModel2.Bettype = 2;
            metaModel2.Language = "th";

            // Assert
            Assert.Equal(123456789, metaModel1.MatchId);
            Assert.Equal(1, metaModel1.Bettype);
            Assert.Equal("en", metaModel1.Language);

            Assert.Equal(987654321, metaModel2.MatchId);
            Assert.Equal(2, metaModel2.Bettype);
            Assert.Equal("th", metaModel2.Language);
        }

        [Fact]
        public void Properties_ShouldBeIndependentlySettable()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();

            // Act & Assert - Set MatchId only
            metaModel.MatchId = 123456789;
            Assert.Equal(123456789, metaModel.MatchId);
            Assert.Equal(0, metaModel.Bettype);
            Assert.Null(metaModel.Language);

            // Act & Assert - Set Bettype only
            metaModel = new SportsResultMetaModel();
            metaModel.Bettype = 5;
            Assert.Equal(0, metaModel.MatchId);
            Assert.Equal(5, metaModel.Bettype);
            Assert.Null(metaModel.Language);

            // Act & Assert - Set Language only
            metaModel = new SportsResultMetaModel();
            metaModel.Language = "fr";
            Assert.Equal(0, metaModel.MatchId);
            Assert.Equal(0, metaModel.Bettype);
            Assert.Equal("fr", metaModel.Language);
        }

        [Fact]
        public void Properties_CanBeResetToDefault()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel
            {
                MatchId = 123456789,
                Bettype = 1,
                Language = "en"
            };

            // Act
            metaModel.MatchId = 0;
            metaModel.Bettype = 0;
            metaModel.Language = null;

            // Assert
            Assert.Equal(0, metaModel.MatchId);
            Assert.Equal(0, metaModel.Bettype);
            Assert.Null(metaModel.Language);
        }

        [Fact]
        public void Properties_CanBeModifiedMultipleTimes()
        {
            // Arrange
            var metaModel = new SportsResultMetaModel();

            // Act & Assert - First modification
            metaModel.MatchId = 111;
            metaModel.Bettype = 1;
            metaModel.Language = "en";
            Assert.Equal(111, metaModel.MatchId);
            Assert.Equal(1, metaModel.Bettype);
            Assert.Equal("en", metaModel.Language);

            // Act & Assert - Second modification
            metaModel.MatchId = 222;
            metaModel.Bettype = 2;
            metaModel.Language = "th";
            Assert.Equal(222, metaModel.MatchId);
            Assert.Equal(2, metaModel.Bettype);
            Assert.Equal("th", metaModel.Language);

            // Act & Assert - Third modification
            metaModel.MatchId = 333;
            metaModel.Bettype = 3;
            metaModel.Language = "zh";
            Assert.Equal(333, metaModel.MatchId);
            Assert.Equal(3, metaModel.Bettype);
            Assert.Equal("zh", metaModel.Language);
        }
    }
}
