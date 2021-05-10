using AutoMapper;
using Domain.Model;
using WebAPI.ViewModel;

namespace WebAPI.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region ViewModel -> Domain

            #region User
            CreateMap<LoginUserViewModel, User>();
            CreateMap<UserCreateViewModel, User>();
            CreateMap<UserAPIViewModel, User>();
            #endregion

            #endregion
        }
    }


}