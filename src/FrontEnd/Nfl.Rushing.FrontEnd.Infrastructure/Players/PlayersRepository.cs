using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using LanguageExt;

using Newtonsoft.Json;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public class PlayersRepository : IPlayersRepository
    {
        public Task<Either<string, IEnumerable<PlayerDto>>> GetAll(
            string sortField,
            SortOrder sortOrder,
            IEnumerable<string> nameFilters)
        {
            return Prelude.TryAsync(
                    async () =>
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.GetAsync(
                                "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

                            var responseContent = await response.Content.ReadAsStringAsync();
                            var rushingPlayers = JsonConvert.DeserializeObject<IEnumerable<PlayerDto>>(
                                responseContent,
                                new PlayerJsonConverter());
                            return rushingPlayers;
                        }
                    })
                .ToEither(error => error.ToString())
                .Map(rushingPlayers => PlayerFilter.FilterByName(rushingPlayers, nameFilters))
                .Map(rushingPlayers => RushingPlayerSorter.Sort(rushingPlayers, sortField, sortOrder))
                .ToEither();
        }
    }
}