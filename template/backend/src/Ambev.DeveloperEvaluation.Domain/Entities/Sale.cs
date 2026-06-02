using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique external identification number generated for the sale.
    /// </summary>
    public Guid SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was completed.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the customer who made the purchase.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the total net amount of the sale after all discounts have been applied.
    /// </summary>
    public decimal TotalSaleAmount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this sale transaction has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets the collection of product items included in this sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        SaleDate = DateTime.UtcNow;
        IsCancelled = false;
    }

    /// <summary>
    /// Performs domain-level validation of the sale aggregate using SaleValidator rules.
    /// </summary>
    /// <returns>A ValidationResultDetail containing execution status and failure reports.</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Cancels the sale transaction and updates its status.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }
}