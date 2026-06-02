using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateSalesRequestProfile : Profile
{
    public CreateSalesRequestProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
    }
}