using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale aggregate root structure.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("The sale must be associated with a valid Customer.");

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("A sale transaction must contain at least one product item.");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
}

/// <summary>
/// Validator for individual SaleItem entities ensuring quantities and constraints align with company policies.
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product validation failed: Product ID cannot be empty.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Product unit price must be greater than zero.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Item quantity must be at least 1 unit.")
            .LessThanOrEqualTo(20).WithMessage("It's not possible to sell above 20 identical items.");

        RuleFor(item => item.Discount)
            .InclusiveBetween(0.0m, 1.0m).WithMessage("Discount percentage rate must be between 0% and 100%.");
    }
}