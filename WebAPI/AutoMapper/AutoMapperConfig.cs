using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Configure(IServiceProvider provider, IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<DomainToViewModelMappingProfile>();
            cfg.AddProfile<ViewModelToDomainMappingProfile>();
            cfg.ConstructServicesUsing(type => ActivatorUtilities.CreateInstance(provider, type));

            //config.AssertConfigurationIsValid();
        }
    }
}
