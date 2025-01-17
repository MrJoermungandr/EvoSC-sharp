﻿using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class LiveModeService : ILiveModeService
{
    private readonly Dictionary<string, string> _availableModes = new()
    {
        {"ta", "Trackmania/TM_TimeAttack_Online.Script.txt"}, 
        {"rounds", "Trackmania/TM_Rounds_Online.Script.txt"},
        {"teams", "Trackmania/TM_Teams_Online.Script.txt"},
        {"cup", "Trackmania/TM_Cup_Online.Script.txt"},
        {"laps", "Trackmania/TM_Laps_Online.Script.txt"},
        {"champion", "Trackmania/TM_Champion_Online.Script.txt"},
        {"ko", "Trackmania/TM_Knockout_Online.Script.txt"}
    };

    private readonly IServerClient _server;
    private readonly IContextService _context;
    private readonly dynamic _locale;

    public LiveModeService(IServerClient server, IContextService context, Locale locale)
    {
        _server = server;
        _context = context;
        _locale = locale;
    }
    
    public IEnumerable<string> GetAvailableModes() => _availableModes.Keys;

    public async Task<string> LoadModeAsync(string mode)
    {
        var modeName = mode;
        
        if (_availableModes.ContainsKey(mode))
        {
            modeName = _availableModes[mode];
        }
        
        await _server.Remote.SetScriptNameAsync(modeName);
        await _server.Remote.RestartMapAsync();

        _context.Audit().Success()
            .WithEventName(AuditEvents.LoadMode)
            .HavingProperties(new {ModeName = modeName})
            .Comment(_locale.Audit_LoadedModeLive);
        
        return modeName;
    }
}
