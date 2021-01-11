using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITest test;

        public TestController(ITest test)
        {
            this.test = test;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hey");
        }
    }
}
