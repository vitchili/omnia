using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly FakeSaleRepository _fakeRepository;
    private readonly FakeSalePricingService _fakePricingService;
    private readonly IMapper _realMapper;
    private readonly CreateSaleHandler _handler;
    private readonly Faker<CreateSaleProductCommand> _productFaker;
    private readonly Faker<CreateSaleCommand> _commandFaker;

    public CreateSaleHandlerTests()
    {
        _fakeRepository = new FakeSaleRepository();
        _fakePricingService = new FakeSalePricingService();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CreateSaleProfile>());
        _realMapper = mapperConfig.CreateMapper();

        _handler = new CreateSaleHandler(_fakeRepository, _fakePricingService, _realMapper);

        _productFaker = new Faker<CreateSaleProductCommand>()
            .RuleFor(p => p.ProductId, f => f.Random.Guid())
            .RuleFor(p => p.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(p => p.UnitPrice, f => f.Random.Decimal(5, 150));

        _commandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(c => c.CustomerId, f => f.Random.Number())
            .RuleFor(c => c.Products, f => _productFaker.Generate(f.Random.Int(1, 4)));
    }

    [Fact(DisplayName = "Given invalid command When handling Then should throw ValidationException")]
    public async Task Handle_WhenCommandIsInvalid_ThrowsValidationException()
    {
        var invalidCommand = _commandFaker.Generate();
        invalidCommand.CustomerId = 0;
        invalidCommand.Products.Clear();

        // Act
        Func<Task> act = async () => await _handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        _fakeRepository.CreateAsyncCalled.Should().BeFalse();
        _fakePricingService.CalculatePricingCalled.Should().BeFalse();
    }

    [Fact(DisplayName = "Given valid command When handling Then should price, save and return mapped result")]
    public async Task Handle_WhenCommandIsValid_ExecutesSuccessfullyAndReturnsResult()
    {
        // Arrange
        var validCommand = _commandFaker.Generate();

        // Act
        var result = await _handler.Handle(validCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(validCommand.CustomerId);

        _fakeRepository.CreateAsyncCalled.Should().BeTrue();
        _fakeRepository.LastSavedSale.Should().NotBeNull();
        _fakeRepository.LastSavedSale!.SaleNumber.Should().NotBe(Guid.Empty);
        _fakeRepository.LastSavedSale.IsCancelled.Should().BeFalse();

        _fakePricingService.CalculatePricingCalled.Should().BeTrue();
    }

    private class FakeSaleRepository : ISaleRepository
    {
        public bool CreateAsyncCalled { get; private set; }
        public Sale? LastSavedSale { get; private set; }

        public Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            CreateAsyncCalled = true;
            LastSavedSale = sale;
            return Task.FromResult(sale);
        }

        public Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Sale?>(null);
        }
    }

    private class FakeSalePricingService : ISalePricingService
    {
        public bool CalculatePricingCalled { get; private set; }

        public void CalculatePricing(Sale sale)
        {
            CalculatePricingCalled = true;
            
            foreach (var item in sale.Items)
            {
                item.TotalAmount = item.Quantity * item.UnitPrice;
            }
            sale.TotalSaleAmount = sale.Items.Sum(i => i.TotalAmount);
        }
    }
}