using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using FakeItEasy;

using FluentAssertions;

using LanguageExt;
using LanguageExt.UnitTesting;

using Newtonsoft.Json;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

using Xunit;

namespace Nfl.Rushing.FrontEnd.Tests.Unit.Infrastructure.Players
{
    public class PlayersRepositoryTests
    {
        private const string ErrorMessage = "Error message";
        private readonly PlayersRepository _sut;
        private readonly IHttpClientFactory _httpClientFactory = A.Fake<IHttpClientFactory>();
        private readonly IFixture _fixture = new Fixture();

        public PlayersRepositoryTests()
        {
            this._sut = new(this._httpClientFactory);
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenHttpClientFactoryThrows_WhenGetAll_ThenReturnsLeft(
            PlayersQuery query,
            Exception exception)
        {
            // Given
            A.CallTo(() => this._httpClientFactory.CreateClient(A<string>._)).Throws(exception);

            // When
            var result = await this._sut.GetAll(query);

            // Then
            result.ShouldBeLeft(x => x.Should().Contain(exception.Message));
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenHttpClientThrows_WhenGetAll_ThenReturnsLeft(
            PlayersQuery query)
        {
            // Given
            A.CallTo(() => this._httpClientFactory.CreateClient(A<string>._)).Returns(new ThrowsHttpClient());

            // When
            var result = await this._sut.GetAll(query);

            // Then
            result.ShouldBeLeft(x => x.Should().Contain(ErrorMessage));
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenHttpClientReturnsInternalServerError_WhenGetAll_ThenReturnsLeft(
            PlayersQuery query)
        {
            // Given
            A.CallTo(() => this._httpClientFactory.CreateClient(A<string>._)).Returns(new TestHttpClient(HttpStatusCode.InternalServerError, ErrorMessage));

            // When
            var result = await this._sut.GetAll(query);

            // Then
            result.ShouldBeLeft();
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenInvalidJsonIsReturned_WhenGetAll_ThenReturnsLeft(
            PlayersQuery query)
        {
            // Given
            A.CallTo(() => this._httpClientFactory.CreateClient(A<string>._)).Returns(new TestHttpClient(HttpStatusCode.OK, ErrorMessage));

            // When
            var result = await this._sut.GetAll(query);

            // Then
            result.ShouldBeLeft();
        }

        [Theory]
        [AutoFakeItEasyData]
        public async Task GivenLessThan1PageOfPlayers_WhenGetPage_ThenReturnsPlayerPageDto(PlayersQuery query)
        {
            // Given
            query.NameFilters = null;
            var players = this._fixture.CreateMany<PlayerDto>(50).ToArray();

            // you need to get the TestResource thing from connect, get the json and return that...
            var serializedPlayers = JsonConvert.SerializeObject(players);
            var res = JsonConvert.DeserializeObject<IEnumerable<PlayerDto>>(serializedPlayers, new PlayerJsonConverter());
            res.Should().BeEquivalentTo(players);

            A.CallTo(() => this._httpClientFactory.CreateClient(A<string>._)).Returns(new TestHttpClient(HttpStatusCode.OK, serializedPlayers));

            // When
            var result = await this._sut.GetPage(query);

            // Then
            result.ShouldBeRight(playersPageDto =>
            {
                playersPageDto.ContinuationToken.Should().BeEmpty();
                playersPageDto.HasMoreResults.Should().BeFalse();
                playersPageDto.Players.Should().BeEquivalentTo(players);
            });
        }


        private class ThrowsHttpClient : HttpClient
        {
            public override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                throw new Exception(ErrorMessage);
            }
        }

        private class TestHttpClient : HttpClient
        {
            private readonly HttpStatusCode _httpStatusCode;
            private readonly string _stringContent;

            public TestHttpClient(HttpStatusCode httpStatusCode, string stringContent)
            {
                this._httpStatusCode = httpStatusCode;
                this._stringContent = stringContent;
            }

            public override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return new HttpResponseMessage(this._httpStatusCode) { Content = new StringContent(this._stringContent) }
                    .AsTask();
            }
        }
    }
}
