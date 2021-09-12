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
                        using (var streamWriter = new StreamWriter(outputStream, leaveOpen: true))
                        using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                        {
                            csvWriter.WriteRecords(data);
                        }

                        outputStream.Position = 0;

                        return Unit.Default;
                    })
                .ToEither(x => x.ToString())
                .AsTask();
        }
    }
}