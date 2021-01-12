using AutoMapper;
using ChattingApp.API.Dtos;
using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration { get; }
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService,
                ILogger<AuthController> logger,
                IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                var userToCreate = _mapper.Map<User>(userForRegisterDto);
                _userService.AddUser(userToCreate);

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var existingUser = _userService.GetUserByEmail(userForLoginDto.Email);
                if (existingUser == null)
                    return BadRequest();

                var token = GenerateToken(existingUser);

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwt:token").Value));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return token;

        }
    }
}
