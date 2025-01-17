﻿using System.Globalization;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;
using LinqToDB;

namespace EvoSC.Common.Database.Repository.Players;

public class PlayerRepository : DbRepository, IPlayerRepository
{
    public PlayerRepository(IDbConnectionFactory dbConnFactory) : base(dbConnFactory)
    {
    }

    public async Task<IPlayer?> GetPlayerByAccountIdAsync(string accountId) => await Table<DbPlayer>()
        .LoadWith(p => p.DbSettings)
        .SingleOrDefaultAsync(t => t.AccountId == accountId);

    public async Task<IPlayer> AddPlayerAsync(string accountId, TmPlayerDetailedInfo playerInfo)
    {
        var player = new DbPlayer
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AccountId = accountId.ToLower(CultureInfo.InvariantCulture),
            NickName = playerInfo.NickName ?? accountId,
            UbisoftName = playerInfo.NickName ?? accountId,
            Zone = playerInfo.Path ?? "World"
        };

        var id = await Database.InsertWithIdentityAsync(player);
        player.Id =  Convert.ToInt64(id);

        var playerSettings = new DbPlayerSettings
        {
            PlayerId = player.Id, 
            DisplayLanguage = "en"
            
        };

        await Database.InsertAsync(playerSettings);

        return player;
    }

    public Task UpdateLastVisitAsync(IPlayer player) => Table<DbPlayer>()
        .Where(t => t.Id == player.Id)
        .Set(t => t.LastVisit, DateTime.UtcNow)
        .UpdateAsync();

    public Task UpdateNicknameAsync(IPlayer player, string newNickname) => Table<DbPlayer>()
        .Where(t => t.Id == player.Id)
        .Set(t => t.NickName, newNickname)
        .UpdateAsync();
}
