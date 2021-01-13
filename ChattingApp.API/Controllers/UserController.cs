using AutoMapper;
using ChattingApp.API.Dtos;
using ChattingApp.Foundation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ChattingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = _userService.GetUserById(currentUserId);
            if (userFromRepo == null)
                return BadRequest();

            var users = _userService.GetAllUser();
            users = users.Where(u => u.Id != currentUserId).ToList();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userService.GetUserById(id);

            var userToReturn = _mapper.Map<UserForListDto>(user);

            return Ok(userToReturn);
        }
    }
}
