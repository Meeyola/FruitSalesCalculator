using System;
using System.Collections.Generic;
using System.Text;

namespace FruitSalesCalculator.Pricing
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(decimal basePrice, decimal quantity);
    }
}
