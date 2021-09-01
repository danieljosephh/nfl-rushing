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
        public Task<Either<string, IEnumerable<RushingPlayerDto>>> GetAll()
        {
            return Prelude.TryAsync(
                    async () =>
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.GetAsync(
                                "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

                            var stats = await response.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<IEnumerable<RushingPlayerDto>>(stats);
                            return res;
                        }
                    })
                .ToEither(error => error.ToString())
                .ToEither();
        }
    }
}