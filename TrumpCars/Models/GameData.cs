using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using TrumpCars.Entities;

namespace TrumpCars.Models
{
    public class GameData
    {
        private readonly TrumpCarDbContext _context = new TrumpCarDbContext();
        public int MaxPlayers { get; set; }
        public int CardCount { get; set; }
        public List<GroupData> Groups { get; set; }

        public AddPlayerResult AddPlayerToGroup(string connectionId)
        {
            var result = new AddPlayerResult();
            var newPlayer = new PlayerGameData {PlayerId = connectionId};
            var group = Groups.FirstOrDefault(g => g.Players.Count < MaxPlayers);
            if (group == null)
            {
                var newGroup = new GroupData { Name = Guid.NewGuid().ToString(), Players = new List<PlayerGameData>(), TrumpCards = new List<TrumpCard>()};
                newGroup.Players.Add(newPlayer);
                Groups.Add(newGroup);
                result.IsGroupFull = false;
                result.GroupName = newGroup.Name;
            }
            else
            {
                group.Players.Add(newPlayer);
                result.IsGroupFull = true;
                result.GroupName = group.Name;
                var trumpCards = GetCards();
                var playerIndex = 0;
                foreach (var playerGameData in group.Players)
                {
                    playerGameData.TrumpCards = trumpCards.Skip(playerIndex*CardCount).Take(CardCount).ToList();
                    ++playerIndex;
                }
            }
            return result;
        }

        public GroupData GetGroupData(string groupName)
        {
            return Groups.FirstOrDefault(g => g.Name == groupName);
        }

        public List<TrumpCard> GetCards()
        {
            var cards = _context.Vehicles.Take(MaxPlayers * CardCount).Select(v => new TrumpCard
            {
                Id = v.Id,
                Title = v.MakeModel,
                ImageUrl = v.ImageUrl,
                CarCharacteristics = v.VehicleCharacteristics.Select(vc => new CarCharacteristic
                {
                    Name = vc.Characteristic.Name,
                    Value = vc.Value
                }).ToList()
            }).ToList();
            return cards;
        }
    }
}