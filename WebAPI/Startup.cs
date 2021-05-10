using Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.AutoMapper;
using WebAPI.Extensions;
using WebAPI.Filtros;

namespace WebAPI
{
    public class Startup
    {        
        private IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Inclui e altera configurações do ServiceCollection
            SetServicesConfiguration(services, this.Configuration, this.Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configura Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }

        public static void SetServicesConfiguration(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // Adiciona controllers
            services.AddControllers();

            // Adiciona Dependências
            services.AddSingleton<IPermissionService, SystemPermissionService>()
                    .AddSingleton<ITokenService, JWTAuthenticationService>()
                    .AddTransient<IRepository<User>, UserStaticRepository>();

            // Configura AutoMapper
            services.AddAutoMapper((provider, cfg) => AutoMapperConfig.Configure(provider, cfg), typeof(Startup));

            // Adiciona JWTAuthorization
            services.AddJwtAuthorization();

            if (environment != null && environment.IsDevelopment())
            {
                // Adiciona Swagger
                services.AddSwaggerDocumentation();
            }

            // Adiciona versionamento de API
            services.AddApiVersioning(options => 
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            // Adiciona filtros de interceptação de requisições
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ErrorResponseFilter));
            });

            // Remove a interceptação implícita da validação do ModelState
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
