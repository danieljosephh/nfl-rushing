using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using LanguageExt;

using Newtonsoft.Json;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayersRepository : IRushingPlayersRepository
    {
        public Task<Either<string, IEnumerable<RushingPlayerDto>>> GetAll(
            string sortField,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            return Prelude.TryAsync(
                    async () =>
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.GetAsync(
                                "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

                            var responseString = await response.Content.ReadAsStringAsync();
                            var rushingPlayers = JsonConvert.DeserializeObject<IEnumerable<RushingPlayerDto>>(
                                responseString,
                                new RushingPlayerJsonConverter());
                            return rushingPlayers;
                        }
                    })
                .ToEither(error => error.ToString())
                .Map(rushingPlayers => RushingPlayerSorter.Sort(rushingPlayers, sortField, sortOrder))
                .ToEither();
        }
    }
}