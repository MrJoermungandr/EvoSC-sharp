﻿using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.Player.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerEventController : EvoScController<IEventControllerContext>
{
    private readonly IPlayerService _playerService;
    
    public PlayerEventController(IPlayerService playerService) => _playerService = playerService;

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args) =>
        _playerService.UpdateAndGreetPlayerAsync(args.Login);
}
