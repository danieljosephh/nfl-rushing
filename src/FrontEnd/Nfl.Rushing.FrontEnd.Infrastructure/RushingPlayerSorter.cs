using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public static class RushingPlayerSorter
    {
        private static readonly Dictionary<string, Func<RushingPlayerDto, object>> PredicateDictionary =
            typeof(RushingPlayerDto).GetProperties()
                .Select(x => x.Name)
                .ToDictionary(x => x, GetRushingPlayerDtoPropertyValue, StringComparer.InvariantCultureIgnoreCase);

        public static IEnumerable<RushingPlayerDto> Sort(
            IEnumerable<RushingPlayerDto> players,
            string sortField,
            SortOrder sortOrder)
        {
            if (PredicateDictionary.ContainsKey(sortField))
            {
                return sortOrder == SortOrder.Ascending
                    ? players.OrderBy(PredicateDictionary[sortField])
                    : players.OrderByDescending(PredicateDictionary[sortField]);
            }

            return players;
        }

        private static Func<RushingPlayerDto, object> GetRushingPlayerDtoPropertyValue(string propertyName)
        {
            return x => typeof(RushingPlayerDto).GetProperty(propertyName)?.GetValue(x);
        }
    }
}