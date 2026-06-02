using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SalePricingServiceTests
{
    private readonly SalePricingService _service;
    private readonly Faker _faker;

    public SalePricingServiceTests()
    {
        _service = new SalePricingService();
        _faker = new Faker();
    }

    [Fact(DisplayName = "Given null sale When calculating pricing Then should throw ArgumentNullException")]
    public void CalculatePricing_WhenSaleIsNull_ThrowsArgumentNullException()
    {
        // Act
        Action act = () => _service.CalculatePricing(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Given product quantity above 20 When calculating pricing Then should throw InvalidOperationException")]
    public void CalculatePricing_WhenQuantityGreaterThan20_ThrowsInvalidOperationException()
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = _faker.Random.Number(),
            Items = new List<SaleItem>
            {
                new() 
                { 
                    ProductId = _faker.Random.Guid(), 
                    Quantity = _faker.Random.Int(21, 100),
                    UnitPrice = _faker.Random.Decimal(10, 50) 
                }
            }
        };

        // Act
        Action act = () => _service.CalculatePricing(sale);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*above 20 identical items*");
    }

    [Theory(DisplayName = "Given valid quantities When calculating pricing Then should apply correct discount tiers")]
    [InlineData(10, 0.20)] 
    [InlineData(15, 0.20)]
    [InlineData(20, 0.20)]
    [InlineData(4, 0.10)] 
    [InlineData(7, 0.10)]
    [InlineData(9, 0.10)]
    [InlineData(1, 0.00)]
    [InlineData(3, 0.00)]
    public void CalculatePricing_ValidQuantities_AppliesCorrectDiscountTier(int quantity, decimal expectedDiscount)
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = _faker.Random.Number(),
            Items = new List<SaleItem>
            {
                new() 
                { 
                    ProductId = _faker.Random.Guid(), 
                    Quantity = quantity, 
                    UnitPrice = _faker.Random.Decimal(10, 100) 
                }
            }
        };

        // Act
        _service.CalculatePricing(sale);

        // Assert
        sale.Items.First().Discount.Should().Be(expectedDiscount);
    }

    [Fact(DisplayName = "Given multiple items When calculating pricing Then should calculate total amounts correctly")]
    public void CalculatePricing_ValidItems_CalculatesTotalSaleAmountCorrectly()
    {
        // Arrange
        var sale = new Sale
        {
            CustomerId = _faker.Random.Number(),
            Items = new List<SaleItem>
            {
                // 5 * 10.00 = 50.00 -> 10% desc = 45.00
                new() { ProductId = _faker.Random.Guid(), Quantity = 5, UnitPrice = 10.00m },
                // 10 * 20.00 = 200.00 -> 20% desc = 160.00
                new() { ProductId = _faker.Random.Guid(), Quantity = 10, UnitPrice = 20.00m },
                // 2 * 5.00 = 10.00 -> 0% desc = 10.00
                new() { ProductId = _faker.Random.Guid(), Quantity = 2, UnitPrice = 5.00m }
            }
        };

        // Act
        _service.CalculatePricing(sale);

        // Assert
        sale.TotalSaleAmount.Should().Be(215.00m);
        sale.Items[0].TotalAmount.Should().Be(45.00m);
        sale.Items[1].TotalAmount.Should().Be(160.00m);
        sale.Items[2].TotalAmount.Should().Be(10.00m);
    }
}