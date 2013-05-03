using System.Collections.Generic;
using AutoMapper;
using MiniAmazon.Domain.Entities;
using MiniAmazon.Web.Models;
using Ninject.Modules;

namespace MiniAmazon.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<AccountInputModel, Account>();
            Mapper.CreateMap<AccountInputModel, Sale>();
            Mapper.CreateMap<CategoryInputModel, Category>();
            Mapper.CreateMap<SaleInputModel, Sale>();
            Mapper.CreateMap<SaleEditRequestInputModel, SaleEditRequest>();

            Mapper.CreateMap<Account, AccountInputModel>();
            Mapper.CreateMap<Category, CategoryInputModel>();
            Mapper.CreateMap<Sale, SaleInputModel>();
            Mapper.CreateMap<SaleEditRequest, SaleEditRequestInputModel>();

            Mapper.CreateMap<Sale, SaleEditRequest>();
        }
    }
}