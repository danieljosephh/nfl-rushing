using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LanguageExt;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayerJsonConverter : JsonConverter<RushingPlayerDto>
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, RushingPlayerDto value, JsonSerializer serializer)
        {
            throw new NotImplementedException("This serializer does not support writing.");
        }

        public override RushingPlayerDto ReadJson(
            JsonReader reader,
            Type objectType,
            RushingPlayerDto existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var playerDto = existingValue ?? new RushingPlayerDto();

            playerDto.Name = token.Value<string>("Player");
            playerDto.Team = token.Value<string>("Team");
            playerDto.Position = token.Value<string>("Pos");
            playerDto.Attempts = token.Value<int>("Att");
            playerDto.AttemptsPerGame = token.Value<int>("Att/G");
            playerDto.Yards = token.Value<string>("Yds")?.Replace(",", string.Empty).Apply(Convert.ToInt32) ?? default(int);
            playerDto.YardsPerAttempt = token.Value<float>("Avg");
            playerDto.YardsPerGame = token.Value<float>("Yds/G");
            playerDto.Touchdowns = token.Value<int>("TD");
            playerDto.LongestRush = token.Value<string>("Lng")?.Replace("T", string.Empty).Apply(Convert.ToInt32) ?? default(int);
            playerDto.WasLongestRushATouchdown = token.Value<string>("Lng")?.Contains("T") ?? default(bool);
            playerDto.FirstDowns = token.Value<int>("1st");
            playerDto.FirstDownsPercentage = token.Value<float>("1st%");
            playerDto.TwentyPlus = token.Value<int>("20+");
            playerDto.FortyPlus = token.Value<int>("40+");
            playerDto.Fumbles = token.Value<int>("FUM");

            return playerDto;
        }
    }
}
