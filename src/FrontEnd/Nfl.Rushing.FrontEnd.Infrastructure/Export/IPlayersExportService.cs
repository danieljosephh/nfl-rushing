using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Export
{
    public interface IPlayersExportService
    {
        Task<Either<string, Unit>> Export(
            string sortField,
            SortOrder sortOrder,
            IEnumerable<string> nameFilters,
            Stream stream);
    }
}