using System;
using System.Collections.Generic;
using System.Linq;

using Castle.Core.Internal;

namespace Nfl.Rushing.FrontEnd.Infrastructure
{
    public static class RushingPlayerFilter
    {
        public static IEnumerable<RushingPlayerDto> FilterByName(
            IEnumerable<RushingPlayerDto> players,
            IEnumerable<string> filterValues)
        {
            var filterValuesArray = filterValues as string[] ?? filterValues.ToArray();
            if (filterValuesArray.IsNullOrEmpty())
            {
                return players;
            }

            return filterValuesArray.Map(
                    filterValue => players.Where(
                        player => string.Equals(player.Name, filterValue, StringComparison.InvariantCultureIgnoreCase)))
                .SelectMany(x => x);
        }
    }
}