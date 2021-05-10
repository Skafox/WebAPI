using Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ITokenService
    {
        string AuthenticateUser(User user);

        TokenValidationParameters GetTokenValidationParameters();
    }
}
