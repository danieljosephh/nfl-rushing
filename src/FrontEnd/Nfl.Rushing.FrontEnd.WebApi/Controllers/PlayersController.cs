using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

namespace Nfl.Rushing.FrontEnd.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersRepository _playersRepository;

        public PlayersController(IPlayersRepository playersRepository)
        {
            this._playersRepository = playersRepository;
        }

        [HttpGet]
        public Task<IActionResult> GetAll(
            [FromQuery] string sortField,
            [FromQuery] SortOrder sortOrder,
            [FromQuery] IEnumerable<string> nameFilters)
        {
            return this._playersRepository.GetAll(sortField ?? string.Empty, sortOrder, nameFilters)
                .ToAsync()
                .Match(x => (IActionResult)this.Ok(x), left => this.StatusCode(500, left));
        }
    }
}