﻿using System.Globalization;
using Config.Net;
using EvoSC.Common.Config.Models;

namespace EvoSC.Common.Config.Stores;

public class EvoScBaseConfigStore : IConfigStore
{
    private readonly TomlConfigStore<IEvoScBaseConfig> _tomlConfigStore;
    private readonly Dictionary<string, string> _cliOptions;
    
    public EvoScBaseConfigStore(string path, Dictionary<string, string> cliOptions)
    {
        _cliOptions = cliOptions;
        _tomlConfigStore = new TomlConfigStore<IEvoScBaseConfig>(path);
    }

    public void Dispose()
    {
        _tomlConfigStore.Dispose();
    }

    public string? Read(string key)
    {
        var configValue = _tomlConfigStore.Read(key);

        if (configValue == null)
        {
            return null;
        }

        var enviKey = key.Replace(".", "_", StringComparison.Ordinal).ToUpper(CultureInfo.InvariantCulture);
        var enviName = $"EVOSC_{enviKey}";
        var enviValue = Environment.GetEnvironmentVariable(enviName);

        _cliOptions.TryGetValue(key, out var cliValue);

        // try cli options, then environment variables and then config file
        return cliValue?.ToString() ?? enviValue ?? configValue;
    }

    public void Write(string key, string? value)
    {
        throw new NotSupportedException();
    }

    public bool CanRead => true;
    public bool CanWrite => false;
}
