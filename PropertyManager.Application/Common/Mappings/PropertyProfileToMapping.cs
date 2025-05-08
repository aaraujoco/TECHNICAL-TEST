using AutoMapper;
using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Entities;

namespace PropertyManager.Application.Common.Mappings;
    public class PropertyProfileToMapping: Profile
    {
        public PropertyProfileToMapping()
        {
            CreateMap<PropertyModel, PropertyManager.Domain.Entities.Property>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.CodeInternal))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));

        CreateMap<PropertyManager.Domain.Entities.Property, PropertyModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.CodeInternal))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));

        CreateMap<PropertyUpdateModel, PropertyManager.Domain.Entities.Property>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.CodeInternal))
               .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
               .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));

        CreateMap<PropertyManager.Domain.Entities.Property, PropertyUpdateModel>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.CodeInternal))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));


        CreateMap<OwnerModel, Owner>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
               .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));

        CreateMap<Owner, OwnerModel>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
               .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));

        CreateMap<PropertyManager.Domain.Entities.Property, PropertyModelOut>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.CodeInternal))
               .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
               .ForPath(dest => dest.Owner!.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
               .AfterMap((src, dest) => {
                if (dest.Owner == null)
                    dest.Owner = new OwnerModelOut { IdOwner = src.IdOwner };
               });

        CreateMap<PropertyTrace, PropertyTraceModel>()
            .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax))
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty));

        CreateMap<PropertyTraceModel, PropertyTrace>()
           .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
           .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax))
           .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty));

        CreateMap<PropertyTrace, PropertyTraceModelOut>()
            .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax))
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.MapFrom(src => src.IdPropertyTrace));

        CreateMap<PropertyTraceModelOut, PropertyTrace>()
           .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
           .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax))
           .ForMember(dest => dest.IdPropertyTrace, opt => opt.MapFrom(src => src.IdPropertyTrace));



    }
}
