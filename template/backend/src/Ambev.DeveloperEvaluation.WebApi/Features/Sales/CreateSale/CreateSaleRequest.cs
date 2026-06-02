namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the list of products included in the sale.
    /// </summary>
    public List<CreateSaleProductRequest> Products { get; set; } = new();
}

/// <summary>
/// Represents a product item within the sale creation request.
/// </summary>
public class CreateSaleProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product being purchased (Max 20).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (Quantity * UnitPrice) before/after application validation.
    /// </summary>
    public decimal TotalAmount { get; set; }
}