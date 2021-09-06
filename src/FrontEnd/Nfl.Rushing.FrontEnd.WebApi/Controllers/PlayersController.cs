using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Nfl.Rushing.FrontEnd.Infrastructure.Export;
using Nfl.Rushing.FrontEnd.Infrastructure.Players;

namespace Nfl.Rushing.FrontEnd.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersExportService _playersExportService;
        private readonly IPlayersRepository _playersRepository;

        public PlayersController(IPlayersRepository playersRepository, IPlayersExportService playersExportService)
        {
            this._playersRepository = playersRepository;
            this._playersExportService = playersExportService;
        }

        [HttpGet]
        public Task<IActionResult> GetAll(
            [FromQuery] string sortField,
            [FromQuery] SortOrder sortOrder,
            [FromQuery] IEnumerable<string> nameFilters)
        {
            return this._playersRepository.GetAll(sortField ?? string.Empty, sortOrder, nameFilters)
                .ToAsync()
                .Match(players => (IActionResult)this.Ok(players), left => this.StatusCode(500, left));
        }

        [HttpGet("export")]
        public Task<IActionResult> Export(
            [FromQuery] string sortField,
            [FromQuery] SortOrder sortOrder,
            [FromQuery] IEnumerable<string> nameFilters)
        {
            var stream = new MemoryStream();
            return this._playersExportService.Export(sortField ?? string.Empty, sortOrder, nameFilters, stream)
                .ToAsync()
                .Do(_ => stream.Position = 0)
                .Match(
                    _ => (IActionResult)this.File(stream, "text/csv", "NflRushingPlayers.csv"),
                    left => this.StatusCode(500, left));
        }
    }
}