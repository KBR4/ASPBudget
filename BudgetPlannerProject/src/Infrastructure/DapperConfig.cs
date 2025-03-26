using Dapper;

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
