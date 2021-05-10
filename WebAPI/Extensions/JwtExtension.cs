using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    /// <summary>
    /// Classe de extensão para configuração para o Jwt Authorization
    /// </summary>
    public static class JwtExtension
    {
        /// <summary>
        /// Configura a autorização para o token jwt
        /// </summary>
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            // Carrega o serviço de token para obter os dados necessários para a configuração do JwtBearer
            IServiceProvider provider = services.BuildServiceProvider();
            ITokenService tokenService = provider.GetRequiredService<ITokenService>();
            TokenValidationParameters paramsValidation = tokenService.GetTokenValidationParameters();

            // Configura a autenticação via Bearer/JWT
            services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                // Adiciona configurações de comportamento de autenticação via Bearer/JWT
                .AddJwtBearer(bearerOptions =>
                {                    
                    // Valida o aud do token
                    paramsValidation.ValidateIssuer = true;
                    paramsValidation.ValidateAudience = true;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;                    
                    paramsValidation.ValidateLifetime = true;
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                    paramsValidation.RoleClaimType = Domain.Enum.CustomClaimTypeEnum.Role.ToString();

                    //bearerOptions.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var path = context.HttpContext.Request.Path;

                    //        if (path.StartsWithSegments("/hub") && context.Request.Query["access_token"] is var accessToken)
                    //        {
                    //            // Read the token out of the query string
                    //            context.Token = accessToken;
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(
                    "Bearer",
                    new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()
                );
            });

            return services;
        }
    }
}
