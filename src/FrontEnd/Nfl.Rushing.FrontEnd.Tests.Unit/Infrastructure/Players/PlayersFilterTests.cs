using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;

using FluentAssertions;

using LanguageExt;

using Nfl.Rushing.FrontEnd.Infrastructure.Players;

using Xunit;

namespace Nfl.Rushing.FrontEnd.Tests.Unit.Infrastructure.Players
{
    public class PlayersFilterTests
    {
        public static IEnumerable<object[]> GetExpectedPlayersAndFilterValuesOption()
        {
            var players = new Fixture().CreateMany<PlayerDto>().ToArray();

            foreach (var player in players)
            {
                yield return new object[]
                {
                    new[] { player.Name }.AsEnumerable().Apply(Prelude.Optional), new[] { player }, players
                };
            }

            yield return new object[]
            {
                players.Take(2).Select(x => x.Name).Apply(Prelude.Optional), players.Take(2), players
            };
            yield return new object[]
            {
                players.Skip(2).Select(x => x.Name).Apply(Prelude.Optional), players.Skip(2), players
            };
            yield return new object[]
            {
                players.Select(x => x.Name).Apply(Prelude.Optional), players, players
            };
        }

        [Theory]
        [AutoFakeItEasyData]
        public void GivenAListOfPlayersAndNoFilterValues_WhenFilterByName_ThenReturnsAllPlayers(PlayerDto[] players)
        {
            // Given
            // When
            var result = PlayersFilter.FilterByName(players, Option<IEnumerable<string>>.None);

            // Then
            result.Should().BeEquivalentTo(players);
        }

        [Theory]
        [InlineAutoFakeItEasyData(null)]
        [InlineAutoFakeItEasyData("")]
        [InlineAutoFakeItEasyData(" ")]
        [InlineAutoFakeItEasyData("some random value")]
        public void GivenAListOfPlayersWithInvalidFilterValue_WhenFilterByName_ThenReturnsNoPlayers(
            string filterValue,
            PlayerDto[] players)
        {
            // Given
            var filterValuesOption = new[] { filterValue }.AsEnumerable().Apply(Prelude.Optional);

            // When
            var result = PlayersFilter.FilterByName(players, filterValuesOption);

            // Then
            result.Should().BeEquivalentTo(Enumerable.Empty<PlayerDto>());
        }

        [Theory]
        [MemberAutoFakeItEasyData(nameof(GetExpectedPlayersAndFilterValuesOption))]
        public void GivenValidFilterValue_WhenFilterByName_ThenReturnsCorrectPlayers(
            Option<IEnumerable<string>> filterValuesOption,
            PlayerDto[] expectedPlayers,
            PlayerDto[] players)
        {
            // Given
            // When
            var result = PlayersFilter.FilterByName(players, filterValuesOption);

            // Then
            result.Should().BeEquivalentTo(expectedPlayers);
        }
    }
}