using FruitSalesCalculator.Pricing;
using Xunit;

namespace FruitSalesCalculator.Tests.Pricing
{
    public class BulkDiscountPricingTests
    {
        private class MockPricingStrategy : IPricingStrategy
        {
            public decimal CalculatePrice(decimal basePrice, decimal quantity)
            {
                return basePrice * quantity;
            }
        }

        [Fact]
        public void Constructor_WithNullInnerStrategy_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BulkDiscountPricing(null, 10m, 15m));
        }

        [Fact]
        public void Constructor_WithNegativeThreshold_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BulkDiscountPricing(innerStrategy, -1m, 10m));

            Assert.Equal("threshold", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithNegativeDiscountPercent_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BulkDiscountPricing(innerStrategy, 10m, -5m));

            Assert.Equal("discountPercent", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithDiscountPercentOver100_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new BulkDiscountPricing(innerStrategy, 10m, 105m));

            Assert.Equal("discountPercent", exception.ParamName);
        }

        [Fact]
        public void CalculatePrice_BelowThreshold_ReturnsFullPrice()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 10m);

            // Act
            var result = pricing.CalculatePrice(10m, 3m);

            // Assert
            Assert.Equal(30m, result); // 10 * 3 = 30, no discount
        }

        [Fact]
        public void CalculatePrice_AtThreshold_AppliesDiscount()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 10m);

            // Act
            var result = pricing.CalculatePrice(10m, 5m);

            // Assert
            Assert.Equal(45m, result); // (10 * 5) - 10% = 50 - 5 = 45
        }

        [Fact]
        public void CalculatePrice_AboveThreshold_AppliesDiscount()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 10m);

            // Act
            var result = pricing.CalculatePrice(10m, 10m);

            // Assert
            Assert.Equal(90m, result); // (10 * 10) - 10% = 100 - 10 = 90
        }

        [Theory]
        [InlineData(1.0, 10.00)]   // Below threshold
        [InlineData(3.0, 27.00)]   // At threshold - discount applied
        [InlineData(5.0, 45.00)]   // Above threshold
        [InlineData(10.0, 90.00)]  // Well above threshold
        public void CalculatePrice_VariousQuantities_ReturnsExpectedPrice(decimal quantity, decimal expectedPrice)
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 3m, 10m);

            // Act
            var result = pricing.CalculatePrice(10m, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePrice_WithZeroDiscount_ReturnsFullPrice()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 0m);

            // Act
            var result = pricing.CalculatePrice(10m, 10m);

            // Assert
            Assert.Equal(100m, result); // No discount applied
        }

        [Fact]
        public void CalculatePrice_With100PercentDiscount_ReturnsFree()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 100m);

            // Act
            var result = pricing.CalculatePrice(10m, 10m);

            // Assert
            Assert.Equal(0m, result); // 100% discount = free
        }

        [Fact]
        public void CalculatePrice_With25PercentDiscount_AppliesCorrectDiscount()
        {
            // Arrange
            var innerStrategy = new MockPricingStrategy();
            var pricing = new BulkDiscountPricing(innerStrategy, 2m, 25m);

            // Act
            var result = pricing.CalculatePrice(8m, 5m);

            // Assert
            Assert.Equal(30m, result); // (8 * 5) - 25% = 40 - 10 = 30
        }

        [Fact]
        public void CalculatePrice_UsesInnerStrategy()
        {
            // Arrange
            var innerStrategy = new StandardPricing();
            var pricing = new BulkDiscountPricing(innerStrategy, 5m, 20m);

            // Act
            var result = pricing.CalculatePrice(5m, 10m);

            // Assert
            Assert.Equal(40m, result); // (5 * 10) - 20% = 50 - 10 = 40
        }
    }
}