using System;
using System.Collections.Generic;
using System.Linq;

using LanguageExt;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public static class PlayersFilter
    {
        public static IEnumerable<PlayerDto> FilterByName(
            IEnumerable<PlayerDto> players,
            Option<IEnumerable<string>> filterValuesOption)
        {
            return filterValuesOption.Match(
                filterValues =>
                    filterValues.Map(
                            filterValue => players.Where(
                                player => string.Equals(
                                    player.Name,
                                    filterValue,
                                    StringComparison.InvariantCultureIgnoreCase)))
                        .SelectMany(x => x),
                () => players);
        }
    }
}