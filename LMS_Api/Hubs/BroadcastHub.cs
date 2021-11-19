using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LMS_Api.Hubs
{
    public class BroadcastHub : Hub
    {
        public async Task SendMessage(string userIds, string message)
        {
            await Clients.Users(userIds).SendAsync("ReceiveMessage", message);
        }
    }
}
