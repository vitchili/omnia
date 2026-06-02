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
        // Mapeamentos de entrada (Command para as Entidades do Domínio)
        // Nota: Mapeia a lista 'Products' do Command para a propriedade correspondente 'Items' da Entidade se aplicável
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));
            
        CreateMap<CreateSaleProductCommand, SaleItem>();

        // Mapeamentos de saída (Entidades do Domínio para o Result do MediatR)
        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
            
        CreateMap<SaleItem, CreateSaleProductResult>();
    }
}