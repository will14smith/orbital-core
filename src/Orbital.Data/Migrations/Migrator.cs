using DbUp.Builder;
using System.Reflection;

namespace Orbital.Data.Migrations
{
    public class Migrator
    {
        public static void Migrate(UpgradeEngineBuilder builder, bool includeSeedData = true)
        {
            var engine = builder
                .WithScriptsEmbeddedInAssembly(typeof(Migrator).GetTypeInfo().Assembly, name => includeSeedData || !name.EndsWith(".seed.sql"))
                .Build();

            engine.PerformUpgrade();
        }
    }
}
