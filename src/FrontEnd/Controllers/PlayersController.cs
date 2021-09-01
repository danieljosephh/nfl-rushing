using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc;

namespace Nfl.Rushing.FrontEnd.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // conve
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(
                "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

            var stats = await response.Content.ReadAsStringAsync();

            return this.Ok(stats);
        }
    }
}
