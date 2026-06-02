using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event published immediately after a sale transaction is successfully created.
/// </summary>
public class SaleCreatedEvent
{
    /// <summary>
    /// Gets the created sale aggregate information.
    /// </summary>
    public Sale Sale { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}