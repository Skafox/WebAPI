using AutoMapper;
using Domain.Enum;
using Domain.Model;
using System;
using WebAPI.ViewModel;

namespace WebAPI.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            #region Domain -> ViewModel

            #region User
            CreateMap<User, LoginUserViewModel>();
            CreateMap<User, UserCreateViewModel>();
            CreateMap<User, UserAPIViewModel>().ForMember(userAPI => userAPI.Permission, opt => opt.MapFrom(user => Enum.GetName(typeof(SystemPermissionEnum), user.Permission)));
            #endregion

            #endregion
        }
    }
}