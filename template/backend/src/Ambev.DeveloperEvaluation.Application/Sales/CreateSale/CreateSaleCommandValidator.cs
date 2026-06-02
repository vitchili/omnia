using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand ensuring core request invariants before processing.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(sale => sale.Products)
            .NotEmpty().WithMessage("The sale must contain at least one product.");

        RuleForEach(sale => sale.Products)
            .SetValidator(new CreateSaleProductCommandValidator());
    }
}

/// <summary>
/// Validator for individual product data inside the command.
/// </summary>
public class CreateSaleProductCommandValidator : AbstractValidator<CreateSaleProductCommand>
{
    public CreateSaleProductCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20).WithMessage("It's not possible to sell above 20 identical items.");
    }
}