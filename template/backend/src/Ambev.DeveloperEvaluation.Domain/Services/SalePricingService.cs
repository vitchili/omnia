using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SalePricingService : ISalePricingService
{
    private class DiscountTier
    {
        public int MinQuantity { get; init; }
        public int MaxQuantity { get; init; }
        public decimal DiscountPercentage { get; init; }
    }

    private readonly List<DiscountTier> _discountRules = new()
    {
        new DiscountTier { MinQuantity = 10, MaxQuantity = 20, DiscountPercentage = 0.20m },
        new DiscountTier { MinQuantity = 4,  MaxQuantity = 9,  DiscountPercentage = 0.10m },
        new DiscountTier { MinQuantity = 1,  MaxQuantity = 3,  DiscountPercentage = 0.00m }
    };

    private const int MaximumAllowedQuantity = 20;

    public void CalculatePricing(Sale sale)
    {
        if (sale == null) throw new ArgumentNullException(nameof(sale));

        foreach (var item in sale.Items)
        {
            if (item.Quantity > MaximumAllowedQuantity)
            {
                throw new InvalidOperationException(
                    $"Business restriction violated: It's not possible to sell above {MaximumAllowedQuantity} identical items for product {item.ProductId}."
                );
            }

            var matchingRule = _discountRules.FirstOrDefault(rule => 
                item.Quantity >= rule.MinQuantity && item.Quantity <= rule.MaxQuantity);

            item.Discount = matchingRule?.DiscountPercentage ?? 0.00m;

            var grossAmount = item.Quantity * item.UnitPrice;
            item.TotalAmount = grossAmount - (grossAmount * item.Discount);
        }

        sale.TotalSaleAmount = sale.Items.Sum(item => item.TotalAmount);
    }
}