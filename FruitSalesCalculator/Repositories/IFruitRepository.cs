using FruitSalesCalculator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FruitSalesCalculator.Repositories
{
    public interface IFruitRepository
    {
      IReadOnlyList<FreshProduce> GetAll();
    }
}
