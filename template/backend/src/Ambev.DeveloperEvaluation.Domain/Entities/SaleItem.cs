using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an individual product item record inside a specific sale aggregate.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the purchase quantity for this specific product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the commercial unit price of the product at the moment of purchase.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the percentage discount applied to this product line based on tier quantity rules.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the calculated total amount for this product item (Quantity * UnitPrice - Discounts).
    /// </summary>
    public decimal TotalAmount { get; set; }
}