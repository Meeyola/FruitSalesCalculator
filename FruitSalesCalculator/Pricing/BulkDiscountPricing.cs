using System;
using System.Collections.Generic;
using System.Text;

namespace FruitSalesCalculator.Pricing
{
    public class BulkDiscountPricing: IPricingStrategy
    {
        private readonly IPricingStrategy _inner;
        private readonly decimal _threshold;
        private readonly decimal _discountPercent;

        public BulkDiscountPricing(IPricingStrategy inner, decimal threshold, decimal discountPercent)
        {

            if (inner is null)
                throw new ArgumentNullException(nameof(inner));
            if (threshold < 0)
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold cannot be negative.");
            if (discountPercent is < 0 or > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercent), "Discount percent must be between 0 and 100.");

            _inner = inner;
            _threshold = threshold;
            _discountPercent = discountPercent;
        }
        public decimal CalculatePrice(decimal basePrice, decimal quantity)
        {
            decimal price = _inner.CalculatePrice(basePrice, quantity);

            if (quantity >= _threshold)
            {
                price -= price * (_discountPercent / 100m);
            }

            return price;
        }
    }
}
