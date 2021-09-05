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
    public class PlayersController : ControllerBase
    {
        private readonly IRushingPlayersRepository _rushingPlayersRepository;

        public PlayersController(IRushingPlayersRepository rushingPlayersRepository)
        {
            this._rushingPlayersRepository = rushingPlayersRepository;
        }

        [HttpGet]
        public Task<IActionResult> GetAll(
            [FromQuery] string sortField = null,
            [FromQuery] SortOrder sortOrder = SortOrder.Ascending,
            [FromQuery] IEnumerable<string> nameFilters = null)
        {
            return this._rushingPlayersRepository.GetAll(sortField ?? string.Empty, sortOrder, nameFilters)
                .ToAsync()
                .Match(x => (IActionResult)this.Ok(x), left => this.StatusCode(500, left));
        }
    }
}