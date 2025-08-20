using Fanex.Athena.BetTicket.Detail.MetaModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.MetaModels
{
    public class ShowingOptionTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var showingOption = new ShowingOption();

            // Assert
            Assert.True(showingOption.GrandTotalVisible);
            Assert.False(showingOption.NGVisible);
            Assert.False(showingOption.BAVisible);
            Assert.False(showingOption.CSVisible);
            Assert.False(showingOption.KNVisible);
            Assert.False(showingOption.ABVisible);
        }

        [Fact]
        public void GrandTotalVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test false
            showingOption.GrandTotalVisible = false;
            Assert.False(showingOption.GrandTotalVisible);

            // Act & Assert - Test true
            showingOption.GrandTotalVisible = true;
            Assert.True(showingOption.GrandTotalVisible);
        }

        [Fact]
        public void BAVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.BAVisible = true;
            Assert.True(showingOption.BAVisible);

            // Act & Assert - Test false
            showingOption.BAVisible = false;
            Assert.False(showingOption.BAVisible);
        }

        [Fact]
        public void CSVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.CSVisible = true;
            Assert.True(showingOption.CSVisible);

            // Act & Assert - Test false
            showingOption.CSVisible = false;
            Assert.False(showingOption.CSVisible);
        }

        [Fact]
        public void CustomerCommVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.CustomerCommVisible = true;
            Assert.True(showingOption.CustomerCommVisible);

            // Act & Assert - Test false
            showingOption.CustomerCommVisible = false;
            Assert.False(showingOption.CustomerCommVisible);
        }

        [Fact]
        public void NGVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.NGVisible = true;
            Assert.True(showingOption.NGVisible);

            // Act & Assert - Test false
            showingOption.NGVisible = false;
            Assert.False(showingOption.NGVisible);
        }

        [Fact]
        public void SBVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.SBVisible = true;
            Assert.True(showingOption.SBVisible);

            // Act & Assert - Test false
            showingOption.SBVisible = false;
            Assert.False(showingOption.SBVisible);
        }

        [Fact]
        public void VSVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.VSVisible = true;
            Assert.True(showingOption.VSVisible);

            // Act & Assert - Test false
            showingOption.VSVisible = false;
            Assert.False(showingOption.VSVisible);
        }

        [Fact]
        public void KNVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.KNVisible = true;
            Assert.True(showingOption.KNVisible);

            // Act & Assert - Test false
            showingOption.KNVisible = false;
            Assert.False(showingOption.KNVisible);
        }

        [Fact]
        public void BTVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.BTVisible = true;
            Assert.True(showingOption.BTVisible);

            // Act & Assert - Test false
            showingOption.BTVisible = false;
            Assert.False(showingOption.BTVisible);
        }

        [Fact]
        public void ABVisible_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.ABVisible = true;
            Assert.True(showingOption.ABVisible);

            // Act & Assert - Test false
            showingOption.ABVisible = false;
            Assert.False(showingOption.ABVisible);
        }

        [Fact]
        public void HasAmountDueToColumn_ShouldBeSettableAndGettable()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act & Assert - Test true
            showingOption.HasAmountDueToColumn = true;
            Assert.True(showingOption.HasAmountDueToColumn);

            // Act & Assert - Test false
            showingOption.HasAmountDueToColumn = false;
            Assert.False(showingOption.HasAmountDueToColumn);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var showingOption = new ShowingOption();

            // Act
            showingOption.GrandTotalVisible = false;
            showingOption.BAVisible = true;
            showingOption.CSVisible = true;
            showingOption.CustomerCommVisible = true;
            showingOption.NGVisible = true;
            showingOption.SBVisible = true;
            showingOption.VSVisible = true;
            showingOption.KNVisible = true;
            showingOption.BTVisible = true;
            showingOption.ABVisible = true;
            showingOption.HasAmountDueToColumn = true;

            // Assert
            Assert.False(showingOption.GrandTotalVisible);
            Assert.True(showingOption.BAVisible);
            Assert.True(showingOption.CSVisible);
            Assert.True(showingOption.CustomerCommVisible);
            Assert.True(showingOption.NGVisible);
            Assert.True(showingOption.SBVisible);
            Assert.True(showingOption.VSVisible);
            Assert.True(showingOption.KNVisible);
            Assert.True(showingOption.BTVisible);
            Assert.True(showingOption.ABVisible);
            Assert.True(showingOption.HasAmountDueToColumn);
        }

        [Fact]
        public void DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var showingOption = new ShowingOption();

            // Assert - Verify all default values
            Assert.True(showingOption.GrandTotalVisible);
            Assert.False(showingOption.NGVisible);
            Assert.False(showingOption.BAVisible);
            Assert.False(showingOption.CSVisible);
            Assert.False(showingOption.KNVisible);
            Assert.False(showingOption.ABVisible);
            
            // Properties without explicit defaults should be false
            Assert.False(showingOption.CustomerCommVisible);
            Assert.False(showingOption.SBVisible);
            Assert.False(showingOption.VSVisible);
            Assert.False(showingOption.BTVisible);
            Assert.False(showingOption.HasAmountDueToColumn);
        }

        [Fact]
        public void MultipleInstances_ShouldHaveIndependentValues()
        {
            // Arrange
            var showingOption1 = new ShowingOption();
            var showingOption2 = new ShowingOption();

            // Act
            showingOption1.GrandTotalVisible = false;
            showingOption1.CustomerCommVisible = true;
            
            showingOption2.GrandTotalVisible = true;
            showingOption2.CustomerCommVisible = false;

            // Assert
            Assert.False(showingOption1.GrandTotalVisible);
            Assert.True(showingOption1.CustomerCommVisible);
            
            Assert.True(showingOption2.GrandTotalVisible);
            Assert.False(showingOption2.CustomerCommVisible);
        }
    }
}
