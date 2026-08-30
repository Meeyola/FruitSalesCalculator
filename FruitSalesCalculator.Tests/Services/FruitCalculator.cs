using FruitSalesCalculator.Models;
using FruitSalesCalculator.Pricing;
using FruitSalesCalculator.PricingService;
using FruitSalesCalculator.Repositories;

namespace FruitSalesCalculator.Tests.PricingService
{
    public class FruitCalculatorTests
    {
        private class MockFruitRepository : IFruitRepository
        {
            private readonly List<FreshProduce> _fruits;
            public MockFruitRepository(List<FreshProduce> fruits)
            {
                _fruits = fruits;
            }
            public List<FreshProduce> GetAll() => _fruits;
        }

        [Fact]
        public void CalculatePrice_WithoutDiscount_ReturnsBasePrice()
        {
            // Arrange
            var fruit = new FreshProduce
            {
                Name = "Apple",
                BasePrice = 2.00m,
                PricingMethod = PricingMethod.PerKg
            };

            var repository = new MockFruitRepository(new List<FreshProduce> { fruit });
            var calculator = new FruitCalculator();

            // Act
            var result = calculator.CalculatePrice(fruit, 3m);

            // Assert
            Assert.Equal(6.00m, result);
        }

        [Fact]
        public void CalculatePrice_WithDiscountThresholdMet_AppliesDiscount()
        {
            // Arrange
            var fruit = new FreshProduce
            {
                Name = "Cherry",
                BasePrice = 5.00m,
                PricingMethod = PricingMethod.PerKg,
                DiscountThreshold = 2.0m,
                DiscountPercentage = 10m
            };

            var repository = new MockFruitRepository(new List<FreshProduce> { fruit });
            var calculator = new FruitCalculator();

            // Act
            var result = calculator.CalculatePrice(fruit, 3m);

            // Assert
            // 3kg * $5.00 = $15.00, 10% discount = $1.50, final = $13.50
            Assert.Equal(13.50m, result);
        }

        [Fact]
        public void CalculatePrice_WithDiscountThresholdNotMet_NoDiscount()
        {
            // Arrange
            var fruit = new FreshProduce
            {
                Name = "Cherry",
                BasePrice = 5.00m,
                PricingMethod = PricingMethod.PerKg,
                DiscountThreshold = 2.0m,
                DiscountPercentage = 10m
            };

            var repository = new MockFruitRepository(new List<FreshProduce> { fruit });
            var calculator = new FruitCalculator();      
                    
            // Act
            var result = calculator.CalculatePrice(fruit, 1.5m);

            // Assert
            // 1.5kg * $5.00 = $7.50, no discount (threshold not met)
            Assert.Equal(7.50m, result);
        }

        [Fact]
        public void CalculatePrice_PerItemPricing_CalculatesCorrectly()
        {
            // Arrange
            var fruit = new FreshProduce
            {
                Name = "Banana",
                BasePrice = 0.30m,
                PricingMethod = PricingMethod.PerItem
            };

            var repository = new MockFruitRepository(new List<FreshProduce> { fruit });
            var calculator = new FruitCalculator();

            // Act
            var result = calculator.CalculatePrice(fruit, 5m);

            // Assert
            Assert.Equal(1.50m, result);
        }

        [Theory]
        [InlineData(1.0, 5.00)]  // Below threshold
        [InlineData(2.0, 9.00)]  // At threshold
        [InlineData(3.0, 13.50)] // Above threshold
        public void CalculatePrice_VariousQuantities_ReturnsExpectedPrice(decimal quantity, decimal expectedPrice)
        {
            // Arrange
            var fruit = new FreshProduce
            {
                Name = "Cherry",
                BasePrice = 5.00m,
                PricingMethod = PricingMethod.PerKg,
                DiscountThreshold = 2.0m,
                DiscountPercentage = 10m
            };

            var repository = new MockFruitRepository(new List<FreshProduce> { fruit });
            var calculator = new FruitCalculator();

            // Act
            var result = calculator.CalculatePrice(fruit, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

    }
}
