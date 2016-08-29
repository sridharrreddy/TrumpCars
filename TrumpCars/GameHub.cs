using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TrumpCars.Controllers;

namespace TrumpCars
{
    public class GameHub : Hub
    {
        public void CharacteristicPick(string roomName, int carId, string name, int value)
        {
            var groupData = HomeController.GameData.GetGroupData(roomName);
            //groupData.

            Clients.OthersInGroup(roomName).compareCharacteristic(name, value);
        }

        public async Task JoinRoom(string roomName)
        {
            var result = HomeController.GameData.AddPlayerToGroup(Context.ConnectionId);
            await Groups.Add(Context.ConnectionId, result.GroupName);
            var groupData = HomeController.GameData.GetGroupData(result.GroupName);
            if (result.IsGroupFull)
            {
                foreach (var playerGameData in groupData.Players)
                {
                    Clients.Client(playerGameData.PlayerId).loadGame(HomeController.GameData.GetClientData(result.GroupName, playerGameData.PlayerId));
                }
            }
            else
            {
                Clients.Client(Context.ConnectionId).loadGame(HomeController.GameData.GetClientData(result.GroupName, Context.ConnectionId));
            }
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}