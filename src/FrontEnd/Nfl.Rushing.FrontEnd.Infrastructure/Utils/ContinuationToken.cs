using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfl.Rushing.FrontEnd.Infrastructure.Utils
{
    public class ContinuationToken<TQuery>
        where TQuery : class
    {
        public TQuery Query { get; set; }
    }
}