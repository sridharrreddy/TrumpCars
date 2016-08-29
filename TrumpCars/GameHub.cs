using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TrumpCars
{
    public class GameHub : Hub
    {
        public void FeaturePick(string roomName, string name, int value)
        {
            Clients.OthersInGroup(roomName).addChatMessage(name, value);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}