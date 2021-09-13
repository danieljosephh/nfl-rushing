using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Export
{
    public class PlayersExportService : IPlayersExportService
    {
        private readonly IExportService _exportService;
        private readonly IPlayersRepository _playersRepository;

        public PlayersExportService(IExportService exportService, IPlayersRepository playersRepository)
        {
            this._exportService = exportService;
            this._playersRepository = playersRepository;
        }

        public Task<Either<string, Unit>> Export(PlayersQuery query, Stream stream)
        {
            return this._playersRepository.GetAll(query).BindAsync(x => this._exportService.Export(x, stream));
        }
    }
}