using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayerDto
    {
        public string Name { get; set; }

        public string Team { get; set; }

        public string Position { get; set; }
        
        public int Attempts { get; set; }
        
        public float AttemptsPerGame { get; set; }
        
        public int Yards { get; set; }
        
        public float YardsPerAttempt { get; set; }
        
        public float YardsPerGame { get; set; }
        
        public int Touchdowns { get; set; }
        
        public int LongestRush { get; set; }
        
        public bool WasLongestRushATouchdown { get; set; }
        
        public int FirstDowns { get; set; }
        
        public float FirstDownsPercentage { get; set; }
        
        public int TwentyPlus { get; set; }
        
        public int FortyPlus { get; set; }
        
        public int Fumbles { get; set; }
    }
}