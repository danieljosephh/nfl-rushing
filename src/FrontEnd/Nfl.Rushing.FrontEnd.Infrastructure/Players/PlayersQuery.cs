using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Players
{
    [DataContract]
    public class PlayersQuery
    {
        [DataMember]
        public string SortField { get; set; } = string.Empty;

        [DataMember]
        public SortOrder SortOrder { get; set; }

        [DataMember]
        public IEnumerable<string> NameFilters { get; set; }

        [DataMember]
        public int StartIndex { get; set; }

        [DataMember]
        public int PageSize { get; set; } = 100;
    }
}