using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public interface IRushingPlayersRepository
    {
        Task<Either<string, IEnumerable<RushingPlayerDto>>> GetAll(
            string sortField,
            SortOrder sortOrder = SortOrder.Ascending);
    }
}