using FruitSalesCalculator.Pricing;

namespace FruitSalesCalculator.Tests.Pricing
{
    public class StandardPricingTests
    {
        [Fact]
        public void CalculatePrice_WithPositiveValues_ReturnsCorrectPrice()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 5.00m;
            decimal quantity = 3m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(15.00m, result);
        }

        [Fact]
        public void CalculatePrice_WithDecimalQuantity_ReturnsCorrectPrice()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 2.50m;
            decimal quantity = 2.5m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(6.25m, result);
        }

        [Fact]
        public void CalculatePrice_WithZeroQuantity_ReturnsZero()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 10.00m;
            decimal quantity = 0m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculatePrice_WithZeroBasePrice_ReturnsZero()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 0m;
            decimal quantity = 5m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void CalculatePrice_WithSmallDecimalValues_MaintainsPrecision()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 0.30m;
            decimal quantity = 5m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(1.50m, result);
        }

        [Theory]
        [InlineData(1.00, 1, 1.00)]
        [InlineData(2.50, 4, 10.00)]
        [InlineData(0.99, 10, 9.90)]
        [InlineData(15.75, 2.5, 39.375)]
        [InlineData(100.00, 0.1, 10.00)]
        public void CalculatePrice_VariousInputs_ReturnsExpectedResults(
            decimal basePrice,
            decimal quantity,
            decimal expectedPrice)
        {
            // Arrange
            var pricing = new StandardPricing();

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePrice_LargeValues_HandlesCorrectly()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 999.99m;
            decimal quantity = 1000m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(999990.00m, result);
        }

        [Fact]
        public void CalculatePrice_VerySmallQuantity_MaintainsPrecision()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 50.00m;
            decimal quantity = 0.01m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(0.50m, result);
        }

        [Fact]
        public void CalculatePrice_ImplementsIPricingStrategy()
        {
            // Arrange
            IPricingStrategy pricing = new StandardPricing();
            decimal basePrice = 3.00m;
            decimal quantity = 2m;

            // Act
            var result = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(6.00m, result);
        }

        [Fact]
        public void CalculatePrice_ConsistentResults_WhenCalledMultipleTimes()
        {
            // Arrange
            var pricing = new StandardPricing();
            decimal basePrice = 4.50m;
            decimal quantity = 3m;

            // Act
            var result1 = pricing.CalculatePrice(basePrice, quantity);
            var result2 = pricing.CalculatePrice(basePrice, quantity);
            var result3 = pricing.CalculatePrice(basePrice, quantity);

            // Assert
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
            Assert.Equal(13.50m, result1);
        }
    }
}