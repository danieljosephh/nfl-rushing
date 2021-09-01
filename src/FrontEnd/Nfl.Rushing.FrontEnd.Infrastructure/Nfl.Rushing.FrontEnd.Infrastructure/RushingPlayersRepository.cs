using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayersRepository : IRushingPlayersRepository
    {
        public async Task<Either<string, IEnumerable<RushingPlayerDto>>> GetAll()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(
                "https://raw.githubusercontent.com/tsicareers/nfl-rushing/master/rushing.json");

            var stats = await response.Content.ReadAsStringAsync();

            throw new Exception();
        }
    }
}