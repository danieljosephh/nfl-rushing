using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Export
{
    public interface IExportService
    {
        Task<Either<string, Unit>> Export<T>(IEnumerable<T> data, Stream outputStream);
    }
}