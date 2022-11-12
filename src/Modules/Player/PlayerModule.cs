﻿using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules;
using EvoSC.Modules.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Modules.Official.Player;

[Module(Name = "Player", Description = "General player handling.", IsInternal = true)]
public class PlayerModule : EvoScModule
{
    
}