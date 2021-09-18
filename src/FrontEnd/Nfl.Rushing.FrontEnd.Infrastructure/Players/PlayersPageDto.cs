using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    public class PlayersPageDto
    {
        public bool HasMoreResults { get; set; }

        public string ContinuationToken { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; }
    }
}