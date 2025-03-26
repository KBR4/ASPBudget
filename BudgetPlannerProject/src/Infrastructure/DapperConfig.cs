using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DapperConfig
    {
        public static void Configure()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
