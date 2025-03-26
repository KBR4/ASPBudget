using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Runner;
namespace Infrastructure.Database
{
    public class MigrationRunner
    {
        private readonly IMigrationRunner _migrationRunner;
        public MigrationRunner(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        public void Run()
        {
            _migrationRunner.MigrateDown(0);
            _migrationRunner.MigrateUp();
        }
    }
}
