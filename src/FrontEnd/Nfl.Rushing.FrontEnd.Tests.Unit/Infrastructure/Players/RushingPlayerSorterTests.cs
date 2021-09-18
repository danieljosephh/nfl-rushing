using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using LanguageExt;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

using Xunit;

namespace Nfl.Rushing.FrontEnd.Tests.Unit.Infrastructure.Players
{
    public class RushingPlayerSorterTests
    {
        public static IEnumerable<object[]> GetAllPlayerFields()
        {
            return typeof(PlayerDto).GetProperties().Select(x => new[] { x.Name });
        }

        public static IEnumerable<object[]> GetInvalidSortFields()
        {
            yield return new object[] { "invalid_sort_field" };
            yield return new object[] { string.Empty };
            yield return new object[] { null };
            yield return new object[] { Option<string>.None };
        }

        [Theory]
        [MemberAutoFakeItEasyData(nameof(GetAllPlayerFields))]
        public void GivenAListOfPlayersASortFieldAndSortAscending_WhenSort_ThenReturnsSortedPlayers(
            string sortField,
            PlayerDto[] players)
        {
            // Given
            // When
            var result = PlayersSorter.Sort(players, sortField, SortOrder.Ascending);

            // Then
            var expected = players.OrderBy(GetRushingPlayerDtoPropertyValue(sortField));
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [MemberAutoFakeItEasyData(nameof(GetAllPlayerFields))]
        public void GivenAListOfPlayersASortFieldAndSortDescending_WhenSort_ThenReturnsSortedPlayers(
            string sortField,
            PlayerDto[] players)
        {
            // Given
            // When
            var result = PlayersSorter.Sort(players, sortField, SortOrder.Descending);

            // Then
            var expected = players.OrderByDescending(GetRushingPlayerDtoPropertyValue(sortField));
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [MemberAutoFakeItEasyData(nameof(GetInvalidSortFields))]
        public void GivenAnInvalidSortField_WhenSort_ThenReturnsSameList(
            Option<string> sortField,
            SortOrder sortOrder,
            PlayerDto[] players)
        {
            // Given
            // When
            var result = PlayersSorter.Sort(players, sortField, sortOrder);

            // Then
            result.Should().BeEquivalentTo(players, options => options.WithStrictOrdering());
        }

        private static Func<PlayerDto, object> GetRushingPlayerDtoPropertyValue(string propertyName)
        {
            return x => typeof(PlayerDto).GetProperty(propertyName)?.GetValue(x);
        }
    }
}