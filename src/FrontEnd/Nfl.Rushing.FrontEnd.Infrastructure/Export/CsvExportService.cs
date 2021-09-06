using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CsvHelper;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Export
{
    public class CsvExportService : IExportService
    {
        public Task<Either<string, Unit>> Export<T>(IEnumerable<T> data, Stream outputStream)
        {
            return Prelude.Try(
                    () =>
                    {
                        using (var writer = new StreamWriter(outputStream, leaveOpen: true))
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(data);
                        }

                        return Unit.Default;
                    })
                .ToEither(x => x.ToString())
                .AsTask();
        }
    }
}