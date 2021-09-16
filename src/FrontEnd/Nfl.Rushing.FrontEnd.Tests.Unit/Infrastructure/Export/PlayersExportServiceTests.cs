using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FakeItEasy;

using FluentAssertions;

using LanguageExt;
using LanguageExt.UnitTesting;

using Nfl.Rushing.FrontEnd.Infrastructure.Export;
using Nfl.Rushing.FrontEnd.Infrastructure.Players;

using Xunit;

namespace Nfl.Rushing.FrontEnd.Tests.Unit.Infrastructure.Export
{
    public class PlayersExportServiceTests
    {
        private readonly IExportService _exportService = A.Fake<IExportService>();
        private readonly IPlayersRepository _playersRepository = A.Fake<IPlayersRepository>();
        private readonly PlayersExportService _sut;

        public PlayersExportServiceTests()
        {
            this._sut = new PlayersExportService(this._exportService, this._playersRepository);
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task
            GivenAPlayersQueryAndStream_WhenExport_ThenPlayersRepositoryAndExportServiceAreCalledWithExpectedArguments(
                PlayersQuery query,
                IEnumerable<PlayerDto> players)
        {
            // Given
            await using var stream = new MemoryStream();
            var playersRepoCall = A.CallTo(() => this._playersRepository.GetAll(query));
            playersRepoCall.Returns(Prelude.Right<string, IEnumerable<PlayerDto>>(players));
            var exportServiceCall = A.CallTo(() => this._exportService.Export(players, stream));
            exportServiceCall.Returns(LanguageExt.Unit.Default);

            // When
            var result = await this._sut.Export(query, stream);

            // Then
            result.ShouldBeRight();
            playersRepoCall.MustHaveHappenedOnceExactly();
            exportServiceCall.MustHaveHappenedOnceExactly();
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task
            GivenAPlayersRepositoryReturnsLeft_WhenExport_ThenReturnsLeft(
                PlayersQuery query,
                string errorMessage)
        {
            // Given
            await using var stream = new MemoryStream();
            A.CallTo(() => this._playersRepository.GetAll(query)).Returns(errorMessage);

            // When
            var result = await this._sut.Export(query, stream);

            // Then
            result.ShouldBeLeft(actualErrorMessage => actualErrorMessage.Should().BeEquivalentTo(errorMessage));
            A.CallTo(() => this._exportService.Export(A<IEnumerable<PlayerDto>>._, A<Stream>._)).MustNotHaveHappened();
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task
            GivenAnExportServiceReturnsLeft_WhenExport_ThenReturnsLeft(
                PlayersQuery query,
                IEnumerable<PlayerDto> players,
                string errorMessage)
        {
            // Given
            await using var stream = new MemoryStream();
            var playersRepoCall = A.CallTo(() => this._playersRepository.GetAll(query));
            playersRepoCall.Returns(Prelude.Right<string, IEnumerable<PlayerDto>>(players));
            A.CallTo(() => this._exportService.Export(players, stream)).Returns(errorMessage);

            // When
            var result = await this._sut.Export(query, stream);

            // Then
            result.ShouldBeLeft(actualErrorMessage => actualErrorMessage.Should().BeEquivalentTo(errorMessage));
            playersRepoCall.MustHaveHappenedOnceExactly();
        }
    }
}