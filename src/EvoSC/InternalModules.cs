using EvoSC.Common.Interfaces;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule;
using EvoSC.Modules.Official.CurrentMapModule;
using EvoSC.Modules.Official.ASayModule;
using EvoSC.Modules.Official.ExampleModule;
using EvoSC.Modules.Official.FastestCp;
using EvoSC.Modules.Official.LiveRankingModule;
using EvoSC.Modules.Official.MapsModule;
using EvoSC.Modules.Official.MatchManagerModule;
using EvoSC.Modules.Official.MatchRankingModule;
using EvoSC.Modules.Official.MatchReadyModule;
using EvoSC.Modules.Official.MatchTrackerModule;
using EvoSC.Modules.Official.MotdModule;
using EvoSC.Modules.Official.NextMapModule;
using EvoSC.Modules.Official.Player;
using EvoSC.Modules.Official.PlayerRecords;
using EvoSC.Modules.Official.Scoreboard;
using EvoSC.Modules.Official.SetName;
using EvoSC.Modules.Official.SpectatorTargetInfoModule;
using EvoSC.Modules.Official.WorldRecordModule;
using FluentMigrator.Runner.Exceptions;

namespace EvoSC;

public static class InternalModules
{
    public static readonly Type[] Modules =
    {
        typeof(PlayerModule),
        typeof(ExampleModule),
        typeof(MapsModule),
        typeof(WorldRecordModule),
        typeof(PlayerRecordsModule),
        typeof(MatchManagerModule),
        typeof(SetNameModule),
        typeof(ScoreboardModule),
        typeof(FastestCpModule),
        typeof(CurrentMapModule),
        typeof(MotdModule),
        typeof(OpenPlanetModule),
        typeof(MatchTrackerModule),
        typeof(MatchReadyModule),
        typeof(NextMapModule),
        typeof(LiveRankingModule),
        typeof(MatchRankingModule),
        typeof(ASayModule),
        typeof(SpectatorTargetInfoModule)
    };

    /// <summary>
    /// Run any migrations from all the modules.
    /// </summary>
    /// <param name="migrations"></param>
    /// <exception cref="Exception"></exception>
    public static void RunInternalModuleMigrations(this IMigrationManager migrations)
    {
        foreach (var module in Modules)
        {
            try
            {
                migrations.MigrateFromAssembly(module.Assembly);
            }
            catch (MissingMigrationsException ex)
            {
                // ignore this as modules don't always have migrations, but we still try to find them
            }
        }
    }
    
    /// <summary>
    /// Load all internal modules.
    /// </summary>
    /// <param name="modules"></param>
    public static async Task LoadInternalModulesAsync(this IModuleManager modules)
    {
        foreach (var module in Modules)
        {
            await modules.LoadAsync(module.Assembly);
        }
    }
}
