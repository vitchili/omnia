using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Domain service responsible for applying pricing policies, validation limits, and discount tiers to sales.
/// </summary>
public interface ISalePricingService
{
    /// <summary>
    /// Processes the sale aggregate, calculating discounts per item and consolidating final totals.
    /// </summary>
    /// <param name="sale">The sale aggregate root to be priced.</param>
    void CalculatePricing(Sale sale);
}