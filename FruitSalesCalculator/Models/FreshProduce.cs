using System;
using System.Collections.Generic;
using System.Text;

namespace FruitSalesCalculator.Models
{
    public class FreshProduce
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public PricingMethod PricingMethod { get; set; }
        public decimal? DiscountThreshold { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
}
