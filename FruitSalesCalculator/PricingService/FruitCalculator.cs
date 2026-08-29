using FruitSalesCalculator.Models;
using FruitSalesCalculator.Pricing;
using FruitSalesCalculator.Repositories;

namespace FruitSalesCalculator.PricingService
{
    public class FruitCalculator
    {
        private readonly IFruitRepository _repository;
 
        public FruitCalculator(IFruitRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<FreshProduce> GetFruits() => _repository.GetAll();

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
