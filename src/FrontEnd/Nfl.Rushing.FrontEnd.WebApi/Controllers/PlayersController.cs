using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Nfl.Rushing.FrontEnd.Infrastructure;

namespace Nfl.Rushing.FrontEnd.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private readonly IRushingPlayersRepository _rushingPlayersRepository;

        public PlayersController(IRushingPlayersRepository rushingPlayersRepository)
        {
            this._rushingPlayersRepository = rushingPlayersRepository;
        }

        [HttpGet]
        public Task<IActionResult> GetAll()
        {
            return this._rushingPlayersRepository.GetAll()
                .ToAsync()
                .Match(x => (IActionResult)this.Ok(x), left => throw new Exception(left));
        }
    }
}