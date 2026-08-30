using FruitSalesCalculator.Models;
using FruitSalesCalculator.Pricing;

namespace FruitSalesCalculator.PricingService
{
    public class FruitCalculator
    {
        public FruitCalculator()
        {
            
        }

        public decimal CalculatePrice(FreshProduce fruit, decimal quantity)
        {
            IPricingStrategy strategy = new StandardPricing();

            if (fruit.DiscountThreshold.HasValue && fruit.DiscountPercentage.HasValue)
            {
                strategy = new BulkDiscountPricing(strategy, fruit.DiscountThreshold.Value, fruit.DiscountPercentage.Value);
            }
            return strategy.CalculatePrice(fruit.BasePrice, quantity);
        }
    }
}
