using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public interface IPlayersRepository
    {
        Task<Either<string, IEnumerable<PlayerDto>>> GetAll(PlayersQuery query);

        Task<Either<string, PlayerPageDto>> GetPage(PlayersQuery query);

        Task<Either<string, PlayerPageDto>> GetNextPage(string continuationToken);
    }
}