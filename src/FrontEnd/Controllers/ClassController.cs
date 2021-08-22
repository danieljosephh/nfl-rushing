using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

namespace Nfl.Rushing.FrontEnd.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClassController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new[] { 1, 2, 3 });
        }
    }
}
