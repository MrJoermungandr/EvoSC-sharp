﻿using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule.Controllers;

[Controller]
public class WorldRecordEventController : EvoScController<IEventControllerContext>
{
    private readonly IWorldRecordService _worldRecordService;

    public WorldRecordEventController(IWorldRecordService worldRecordService) =>
        _worldRecordService = worldRecordService;

    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public Task OnMapEndAsync(object sender, MapEventArgs mapEventArgs)
        => _worldRecordService.ClearRecordAsync();

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public Task OnMapStartAsync(object sender, MapEventArgs mapEventArgs)
        => _worldRecordService.FetchRecordAsync(mapEventArgs.Map.Uid);
}
