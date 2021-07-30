using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIApplication2.Models;
using WebAPIApplication2.DTOs;

namespace WebAPIApplication2.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderCreateDto>();
            CreateMap<OrderCreateDto, Order>();
        }
    }
}
