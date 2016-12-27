using DbUp.Builder;
using System.Reflection;

namespace Orbital.Data.Migrations
{
    public class Migrator
    {
        public static void Migrate(UpgradeEngineBuilder builder)
        {
            var engine = builder
                .WithScriptsEmbeddedInAssembly(typeof(Migrator).GetTypeInfo().Assembly)
                .Build();

            engine.PerformUpgrade();
        }
    }
}
