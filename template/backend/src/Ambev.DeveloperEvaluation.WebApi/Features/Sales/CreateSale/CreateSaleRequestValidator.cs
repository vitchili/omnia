using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.Products)
            .NotEmpty().WithMessage("The sale must contain at least one product.");

        RuleForEach(sale => sale.Products)
            .SetValidator(new CreateSaleProductRequestValidator());
    }
}

/// <summary>
/// Validator for individual product items inside the CreateSaleRequest.
/// </summary>
public class CreateSaleProductRequestValidator : AbstractValidator<CreateSaleProductRequest>
{
    public CreateSaleProductRequestValidator()
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