using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Mi9DataAccessLayer;
using TFS_API.Models.DTO;

namespace TFS_API.Models.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<product, ProductDTOVM>();
            //CreateMap<rpricehdr, RetailPriceDTO>();
        }
    }
}