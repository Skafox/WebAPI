using AutoMapper;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions;
using WebAPI.ViewModel;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _JWTauthenticationService;

        public LoginController(ILogger<UserController> logger, IMapper mapper, ITokenService JWTauthenticationService)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._JWTauthenticationService = JWTauthenticationService;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginUserViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                // Cria um Exame Modelo a partir do viewmodel recebido do acionamento da API
                User user = this._mapper.Map<User>(userLogin);

                // Devolve um token JWT
                return Ok(this._JWTauthenticationService.AuthenticateUser(user));
            }
            return BadRequest(ModelState.ErrorResponse());
        }

    }
}
