﻿using AutoMapper;
using DataAccess.Entities;
using GenderHealcareSystem.DTO;
using GenderHealcareSystem.DTO.Request;

namespace GenderHealcareSystem.CustomActionFilters
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<Service, AddServiceRequest>().ReverseMap();
            CreateMap<Service, UpdateServiceRequest>().ReverseMap();
            CreateMap<Blog, BlogDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
            .ReverseMap();
            CreateMap<Blog, AddBlogRequest>().ReverseMap();
            CreateMap<Blog, UpdateBlogRequest>().ReverseMap();
            CreateMap<UpdateUserRequest, User>().ForMember(dest => dest.Birthday,
                opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<User, StaffConsultantDto>().ReverseMap();
            CreateMap<AddStaffConsultantRequest, User>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));
            CreateMap<User, UpdateStaffConsultantRequest>().ReverseMap();
        }
    }
}
