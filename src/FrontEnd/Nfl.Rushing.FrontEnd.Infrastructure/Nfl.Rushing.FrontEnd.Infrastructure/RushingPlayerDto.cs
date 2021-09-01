using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public class RushingPlayerDto
    {
        public int Name { get; set; }

        public int Team { get; set; }

        public int Position { get; set; }

        public float AttemptsPerGame { get; set; }

        public int Attemps { get; set; }

        public int TotalYards { get; set; }

        public int YardsPerAttempt { get; set; }

        public int YardsPerGame { get; set; }

        public int Touchdowns { get; set; }

        public int LongestRush { get; set; }

        public int FirstDowns { get; set; }

        public double FirstDownsPercentage { get; set; }

        public int TwentyPlus { get; set; }

        public int FortyPlus { get; set; }

        public int Fumbles { get; set; }
    }
}