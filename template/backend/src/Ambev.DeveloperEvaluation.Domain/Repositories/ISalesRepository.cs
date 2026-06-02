using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity aggregate operations.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Persists a finalized sale aggregate record into the repository.
    /// </summary>
    /// <param name="sale">The sale entity instance to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The persisted sale entity with updated tracker state.</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific sale transaction by its unique database identifier.
    /// </summary>
    /// <param name="id">The unique ID of the sale aggregate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale aggregate if found; otherwise, null.</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}