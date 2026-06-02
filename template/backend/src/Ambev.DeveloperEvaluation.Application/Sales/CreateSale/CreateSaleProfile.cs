using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Sale entities and Application Use Case primitives.
/// </summary>
public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));
            
        CreateMap<CreateSaleProductCommand, SaleItem>();

        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<SaleItem, CreateSaleProductResult>();
    }
}