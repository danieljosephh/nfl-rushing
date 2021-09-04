using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public static class RushingPlayerSorter
    {
        private static readonly Lazy<HashSet<string>> RushingPlayerDtoProperties = new(
            () => typeof(RushingPlayerDto).GetProperties()
                .Select(x => x.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase));

        private static readonly Dictionary<string, Func<RushingPlayerDto, object>> SortFieldDictionary = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { nameof(RushingPlayerDto.Name), x => x.Name },
            { nameof(RushingPlayerDto.Team), x => x.Team },
            { nameof(RushingPlayerDto.Position), x => x.Position },
            { nameof(RushingPlayerDto.Attempts), x => x.Attempts },
            { nameof(RushingPlayerDto.AttemptsPerGame), x => x.AttemptsPerGame },
            { nameof(RushingPlayerDto.Yards), x => x.Yards },
            { nameof(RushingPlayerDto.YardsPerAttempt), x => x.YardsPerAttempt },
            { nameof(RushingPlayerDto.YardsPerGame), x => x.YardsPerGame },
            { nameof(RushingPlayerDto.Touchdowns), x => x.Touchdowns },
            { nameof(RushingPlayerDto.LongestRush), x => x.LongestRush },
            { nameof(RushingPlayerDto.WasLongestRushATouchdown), x => x.WasLongestRushATouchdown },
            { nameof(RushingPlayerDto.FirstDowns), x => x.FirstDowns },
            { nameof(RushingPlayerDto.FirstDownsPercentage), x => x.FirstDownsPercentage },
            { nameof(RushingPlayerDto.TwentyPlus), x => x.TwentyPlus },
            { nameof(RushingPlayerDto.FortyPlus), x => x.FortyPlus },
            { nameof(RushingPlayerDto.Fumbles), x => x.Fumbles }
        };

        public static IEnumerable<RushingPlayerDto> Sort(
            IEnumerable<RushingPlayerDto> players,
            string sortField,
            SortOrder sortOrder)
        {
            if (!RushingPlayerDtoProperties.Value.Contains(sortField))
            {
                return players;
            }

            return sortOrder == SortOrder.Ascending
                ? players.OrderBy(SortFieldDictionary[sortField])
                : players.OrderByDescending(SortFieldDictionary[sortField]);
        }
    }
}