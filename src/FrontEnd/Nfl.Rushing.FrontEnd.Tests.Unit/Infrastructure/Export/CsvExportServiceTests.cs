using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CsvHelper;

using FluentAssertions;

using LanguageExt.UnitTesting;

using Nfl.Rushing.FrontEnd.Infrastructure.Export;
using Nfl.Rushing.FrontEnd.Infrastructure.Players;

using Xunit;

namespace Nfl.Rushing.FrontEnd.Tests.Unit.Infrastructure.Export
{
    public class CsvExportServiceTests
    {
        private readonly CsvExportService _sut = new();

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenAListOfItems_WhenExport_ThenItemsAreExportedToStream(
            IEnumerable<PlayerDto> players)
        {
            // Given
            using var memoryStream = new MemoryStream();

            // When
            var result = await this._sut.Export(players, memoryStream);

            result.ShouldBeRight();
            memoryStream.Position.Should().Be(0);
            using (var streamWriter = new StreamReader(memoryStream))
            using (var csvReader = new CsvReader(streamWriter, CultureInfo.InvariantCulture))
            {
                // Then
                var actual = csvReader.GetRecords(typeof(PlayerDto));
                actual.Should().BeEquivalentTo(players);
            }
        }
    }
}