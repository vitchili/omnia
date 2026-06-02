namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateSale operation
/// </summary>
public class CreateSaleResponse
{
    /// </summary>
    public Guid SaleNumber { get; set; }

    /// <summary>
    /// The date and time when the sale was completed.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The unique identifier of the customer.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Total monetary amount of the sale after all discounts have been applied.
    /// </summary>
    public decimal TotalSaleAmount { get; set; }

    /// <summary>
    /// Indicates whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// List of products processed in this sale.
    /// </summary>
    public List<CreateSaleProductResponse> Products { get; set; } = new();
}

/// <summary>
/// API response model for product details in the sale context.
/// </summary>
public class CreateSaleProductResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// The discount amount applied to this item tier.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item after discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }
}