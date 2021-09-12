using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using LanguageExt;

using Newtonsoft.Json;

using Nfl.Rushing.FrontEnd.Infrastructure.Utils;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayersRepository(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public Task<Either<string, IEnumerable<PlayerDto>>> GetAll(PlayersQuery query)
        {
            return Prelude.TryAsync(this.GetPlayersAndMapToDto)
                .ToEither(error => error.ToString())
                .Map(rushingPlayers => PlayerFilter.FilterByName(rushingPlayers, query.NameFilters))
                .Map(rushingPlayers => RushingPlayerSorter.Sort(rushingPlayers, query.SortField, query.SortOrder))
                .ToEither();
        }

        public Task<Either<string, PlayerPageDto>> GetPage(PlayersQuery query)
        {
            return this.GetAll(query).ToAsync().Map(x => Paginate(x.ToArray(), query)).ToEither();
        }

        public Task<Either<string, PlayerPageDto>> GetNextPage(string continuationToken)
        {
            return Base64ConversionHelper.ConvertFromBase64String<ContinuationToken<PlayersQuery>>(continuationToken)
                .Apply(token => this.GetPage(token.Query));
        }

        private static PlayerPageDto Paginate(IReadOnlyCollection<PlayerDto> players, PlayersQuery query)
        {
            var hasMoreResults = players.Count - query.StartIndex > query.PageSize;
            return new PlayerPageDto
            {
                HasMoreResults = hasMoreResults,
                Players = players.Skip(query.StartIndex).Take(query.PageSize),
                ContinuationToken = hasMoreResults ? BuildContinuationToken(query) : string.Empty
            };
        }

        private static string BuildContinuationToken(PlayersQuery query)
        {
            query.StartIndex += query.PageSize;
            return new ContinuationToken<PlayersQuery> { Query = query }.Apply(
                Base64ConversionHelper.ConvertToBase64String);
        }

        private async Task<IEnumerable<PlayerDto>> GetPlayersAndMapToDto()
        {
            using (var httpClient = this._httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync(
                    "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

                var responseContent = await response.Content.ReadAsStringAsync();
                var rushingPlayers = JsonConvert.DeserializeObject<IEnumerable<PlayerDto>>(
                    responseContent,
                    new PlayerJsonConverter());
                return rushingPlayers;
            }
        }
    }
}