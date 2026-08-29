using System;
using System.Collections.Generic;
using System.Text;

namespace FruitSalesCalculator.Pricing
{
    public class StandardPricing: IPricingStrategy
    {
        public decimal CalculatePrice(decimal basePrice, decimal quantity)
        {
            return basePrice * quantity;
        }
    }
}
