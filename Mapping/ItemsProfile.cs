using AutoMapper;
using Catalog.Domain;
using Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Mapping
{
    public class ItemsProfile : Profile //Inherit from base class
    {
        public ItemsProfile()
        {
            CreateMap<Item, ItemDto>(); //Map between source & destination object.
            CreateMap<CreatItemDto, Item>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
        }
    }
}
