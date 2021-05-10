using AutoMapper;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions;
using WebAPI.ViewModel;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _repository;

        public UserController(ILogger<UserController> logger, IMapper mapper, IRepository<User> repository)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok(_repository.ById(id));
        }

        [HttpGet]
        public IActionResult GetAll(long id)
        {
            return Ok(_repository.All());
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                User usuarioModelo = _repository.Add(_mapper.Map<User>(user));
                if (usuarioModelo.ID.HasValue)
                {
                    return Created($"/api/user/{usuarioModelo.ID}", usuarioModelo);
                }
            }
            return BadRequest(ModelState.ErrorResponse());
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserUpdateViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_repository.Update(_mapper.Map<User>(user)))
                    return Ok();
            }
            return BadRequest(ModelState.ErrorResponse());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            if (_repository.ById(id) == null)
                return NotFound();

            if (_repository.Delete(id))
                return NoContent();

            return BadRequest();
        }

    }
}
