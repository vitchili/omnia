namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents the result returned after successfully persisting a new sale transaction.
/// </summary>
public class CreateSaleResult
{
    public Guid SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalSaleAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<CreateSaleProductResult> Products { get; set; } = new();
}

/// <summary>
/// Contains structured output details for items associated with the finalized sale.
/// </summary>
public class CreateSaleProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}