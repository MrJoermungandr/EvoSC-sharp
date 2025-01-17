﻿using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class OpenPlanetScheduler : IOpenPlanetScheduler
{
    private readonly IOpenPlanetControlSettings _opSettings;
    
    private readonly Dictionary<string, (DateTime, IPlayer)> _scheduledKicks = new();
    private readonly IEventManager _events;
    private readonly object _scheduledKicksLock = new();

    public OpenPlanetScheduler(IOpenPlanetControlSettings opSettings, IEventManager events)
    {
        _opSettings = opSettings;
        _events = events;
    }
    
    public void ScheduleKickPlayer(IPlayer player, bool renew)
    {
        lock (_scheduledKicksLock)
        {
            if (!_scheduledKicks.ContainsKey(player.AccountId) || renew)
            {
                _scheduledKicks[player.AccountId] = (DateTime.Now, player);
            }
        }
    }

    public bool UnScheduleKickPlayer(IPlayer player)
    {
        lock (_scheduledKicksLock)
        {
            return _scheduledKicks.Remove(player.AccountId, out _);
        }
    }

    public async Task TriggerDuePlayerKicksAsync()
    {
        var duePlayers = new List<IPlayer>();
        lock (_scheduledKicksLock)
        {
            foreach (var (accountId, (time, player)) in _scheduledKicks)
            {
                if (time + TimeSpan.FromSeconds(_opSettings.KickTimeout) > DateTime.Now)
                {
                    continue;
                }

                _scheduledKicks.Remove(accountId, out _);
                duePlayers.Add(player);
            }
        }

        foreach (var player in duePlayers)
        {
            await _events.RaiseAsync(
                OpenPlanetEvents.PlayerDueForKick,
                new PlayerDueForKickEventArgs {Player = player}
            );
        }
    }

    public bool PlayerIsScheduledForKick(IPlayer player)
    {
        lock (_scheduledKicksLock)
        {
            return _scheduledKicks.ContainsKey(player.AccountId);
        }
    }
}
