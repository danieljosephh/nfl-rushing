using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayerDto
    {
        [JsonProperty(PropertyName = "Player")]
        public string Name { get; set; }

        //[JsonProperty(PropertyName = "Team")]
        public string Team { get; set; }

        [JsonProperty(PropertyName = "Pos")]
        public string Position { get; set; }
        
        [JsonProperty(PropertyName = "Att")]
        public int Attempts { get; set; }
        
        [JsonProperty(PropertyName = "Att/G")]
        public float AttempsPerGame { get; set; }
        
        [JsonProperty(PropertyName = "Yds")]
        public string Yards { get; set; }
        
        [JsonProperty(PropertyName = "Avg")]
        public float YardsPerAttempt { get; set; }
        
        [JsonProperty(PropertyName = "Yds/G")]
        public float YardsPerGame { get; set; }
        
        [JsonProperty(PropertyName = "TD")]
        public int Touchdowns { get; set; }
        
        [JsonProperty(PropertyName = "Lng")]
        public string LongestRush { get; set; }
        
        [JsonProperty(PropertyName = "1st")]
        public int FirstDowns { get; set; }
        
        [JsonProperty(PropertyName = "1st%")]
        public float FirstDownsPercentage { get; set; }
        
        [JsonProperty(PropertyName = "20+")]
        public int TwentyPlus { get; set; }
        
        [JsonProperty(PropertyName = "40+")]
        public int FortyPlus { get; set; }
        
        [JsonProperty(PropertyName = "FUM")]
        public int Fumbles { get; set; }
    }
}