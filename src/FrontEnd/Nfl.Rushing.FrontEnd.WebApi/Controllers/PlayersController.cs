using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

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
        [ProducesResponseType(typeof(PlayersPageDto), 200)]
        public Task<IActionResult> Get(
            [FromQuery] PlayersQuery query,
            [FromQuery] string continuationToken = null)
        {
            return continuationToken == null
                ? this.MapToResponse(() => this._playersRepository.GetPage(query))
                : this.MapToResponse(() => this._playersRepository.GetNextPage(continuationToken));
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        public Task<IActionResult> Export([FromQuery] PlayersQuery query)
        {
            var stream = new MemoryStream();
            return this._playersExportService.Export(query, stream)
                .ToAsync()
                .Match(
                    _ => (IActionResult)this.File(stream, "text/csv", "NflRushingPlayers.csv"),
                    left => this.StatusCode(500, left));
        }

        private Task<IActionResult> MapToResponse(Func<Task<Either<string, PlayersPageDto>>> repositoryCall)
        {
            return repositoryCall()
                .ToAsync()
                .Match(players => (IActionResult)this.Ok(players), left => this.StatusCode(500, left));
        }
    }
}