using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests by orchestrating infrastructure and domain operations.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISalePricingService _salePricingService;
    private readonly IMapper _mapper;

    public CreateSaleHandler(
        ISaleRepository saleRepository, 
        ISalePricingService salePricingService, 
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _salePricingService = salePricingService;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = _mapper.Map<Sale>(command);

        sale.SaleNumber = Guid.NewGuid();
        sale.SaleDate = DateTime.UtcNow;
        sale.IsCancelled = false;

        _salePricingService.CalculatePricing(sale);

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        
        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}