using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public static class RushingPlayerSorter
    {
        private static readonly Dictionary<string, Func<PlayerDto, object>> PredicateDictionary =
            typeof(PlayerDto).GetProperties()
                .Select(x => x.Name)
                .ToDictionary(x => x, GetRushingPlayerDtoPropertyValue, StringComparer.InvariantCultureIgnoreCase);

        public static IEnumerable<PlayerDto> Sort(
            IEnumerable<PlayerDto> players,
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

        private static Func<PlayerDto, object> GetRushingPlayerDtoPropertyValue(string propertyName)
        {
            return x => typeof(PlayerDto).GetProperty(propertyName)?.GetValue(x);
        }
    }
}