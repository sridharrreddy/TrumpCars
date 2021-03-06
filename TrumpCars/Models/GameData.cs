﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrumpCars.Entities;

namespace TrumpCars.Models
{
    public class GameData
    {
        private readonly TrumpCarDbContext _context = new TrumpCarDbContext();
        public int MaxPlayers { get; set; }
        public int CardCount { get; set; }
        public List<GroupData> Groups { get; set; }

        public AddPlayerResult AddPlayerToGroup(string playerId)
        {
            var result = new AddPlayerResult();
            var newPlayer = new PlayerGameData { PlayerId = playerId };
            var group = Groups.FirstOrDefault(g => g.Players.Count < MaxPlayers);
            if (group == null)
            {
                var newGroup = new GroupData { Name = Guid.NewGuid().ToString(), Players = new List<PlayerGameData>() };
                newPlayer.IsPlayersTurn = true;
                newGroup.Players.Add(newPlayer);
                Groups.Add(newGroup);
                result.IsGroupFull = false;
                result.GroupName = newGroup.Name;
            }
            else
            {
                group.Players.Add(newPlayer);
                result.IsGroupFull = group.Players.Count() == this.MaxPlayers;
                result.GroupName = group.Name;
                var trumpCards = GetCards();
                var playerIndex = 0;
                foreach (var playerGameData in group.Players)
                {
                    playerGameData.TrumpCards = trumpCards.Skip(playerIndex * CardCount).Take(CardCount).ToList();
                    playerGameData.TrumpCards.First().IsActive = true;
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
            var cards = _context.Vehicles
                .Where(v => v.VehicleCharacteristics.Count() > 4)
                .OrderBy(c => Guid.NewGuid())
                .Take(MaxPlayers * CardCount)
                .Select(v => new TrumpCard
                {
                    Id = v.Id,
                    Title = v.MakeModel,
                    ImageUrl = v.ImageUrl,
                    Finished = false,
                    CarCharacteristics = v.VehicleCharacteristics
                        .Where(vc => vc.Characteristic.Name == "Price (AUD)"
                                     || vc.Characteristic.Name == "Engine Size (cc)"
                                     || vc.Characteristic.Name == "Fuel Star Rating"
                                     || vc.Characteristic.Name == "Performance"
                                     || vc.Characteristic.Name == "Boot Space Score")
                        .Select(vc => new CarCharacteristic
                        {
                            Name = vc.Characteristic.Name,
                            Value = vc.Value
                        }).ToList()
                }).ToList();
            return cards;
        }

        public void MakePick(string groupName, string playerId, int carId, string name)
        {
            var group = GetGroupData(groupName);
            var activePlayer = group.Players.First(p => p.IsPlayersTurn);
            var redundantCheck = activePlayer.PlayerId == playerId;
            var activeCard = activePlayer.TrumpCards.First(c => c.IsActive);
            activeCard.CarCharacteristics.First(cc => cc.Name == name).IsPicked = true;

            var opponentPlayer = group.Players.First(p => !p.IsPlayersTurn);
            var opponentActiveCard = opponentPlayer.TrumpCards.First(c => c.IsActive);
            opponentActiveCard.CarCharacteristics.First(cc => cc.Name == name).IsPicked = true;

            if (activeCard.CarCharacteristics.First(cc => cc.Name == name).Value >
                opponentActiveCard.CarCharacteristics.First(cc => cc.Name == name).Value)
            {
                activePlayer.Score++;
                activeCard.Win = true;
            }
            else if (activeCard.CarCharacteristics.First(cc => cc.Name == name).Value <
                     opponentActiveCard.CarCharacteristics.First(cc => cc.Name == name).Value)
            {
                opponentPlayer.Score++;
                opponentActiveCard.Win = true;
            }
            else
            {
                group.IsDraw = true;
            }
            if (activePlayer.TrumpCards.Count(c => !c.Finished) == 1)
            {
                NextRound(groupName);
            }
        }

        public void NextRound(string groupName)
        {
            var group = GetGroupData(groupName);

            var activePlayer = group.Players.First(p => p.IsPlayersTurn);
            var activeCard = activePlayer.TrumpCards.First(c => c.IsActive);

            var opponentPlayer = group.Players.First(p => !p.IsPlayersTurn);
            var opponentActiveCard = opponentPlayer.TrumpCards.First(c => c.IsActive);

            activePlayer.IsPlayersTurn = false;
            activeCard.Finished = true;
            activeCard.IsActive = false;
            activeCard.CarCharacteristics.First(cc => cc.IsPicked).IsPicked = false;

            opponentPlayer.IsPlayersTurn = true;
            opponentActiveCard.Finished = true;
            opponentActiveCard.IsActive = false;
            opponentActiveCard.CarCharacteristics.First(cc => cc.IsPicked).IsPicked = false;
        }

        public string GetClientData(string groupName, string playerId)
        {
            var groupData = GetGroupData(groupName);

            var playerData = groupData.Players.First(p => p.PlayerId == playerId);
            TrumpCard activeCard = null;
            if (playerData.TrumpCards.Any(c => !c.Finished))
            {
                activeCard = playerData.TrumpCards.First(c => !c.Finished);
                activeCard.IsActive = true;
            }

            var opponentData = groupData.Players.First(p => p.PlayerId != playerId);
            TrumpCard opponentsActiveCard = opponentData.TrumpCards.FirstOrDefault(c => c.CarCharacteristics.Any(cc => cc.IsPicked));

            var result = new
            {
                inGame = groupData.Players.Count == MaxPlayers,
                roomName = groupName,
                currentGame = new
                {
                    isGameFinished = playerData.TrumpCards.All(c => c.Finished),
                    counter = playerData.TrumpCards.Count(c => !c.Finished),
                    thisRound = new
                    {
                        myTurn = playerData.IsPlayersTurn,
                        myCard = activeCard == null ? null : new
                        {
                            Result = opponentsActiveCard == null ? "" : (activeCard.Win ? "Win" : (groupData.IsDraw ? "Draw" : "Lose")),
                            activeCard.Id,
                            activeCard.Title,
                            activeCard.ImageUrl,
                            CarCharacteristics = activeCard.CarCharacteristics.Select(c => new
                            {
                                c.Name,
                                c.Value,
                                c.IsPicked
                            }).ToList()
                        },
                        opponentsCard = opponentsActiveCard == null ? null : new
                        {
                            Result = opponentsActiveCard.Win ? "Win" : (groupData.IsDraw ? "Draw" : "Lose"),
                            opponentsActiveCard.Id,
                            opponentsActiveCard.Title,
                            opponentsActiveCard.ImageUrl,
                            CarCharacteristics = opponentsActiveCard.CarCharacteristics.Select(c => new
                            {
                                c.Name,
                                c.Value,
                                c.IsPicked
                            }).ToList()
                        },
                        myScore = playerData.Score,
                        opponentsScore = opponentData.Score
                    }
                }
            };

            return JsonConvert.SerializeObject(result);
        }
    }
}