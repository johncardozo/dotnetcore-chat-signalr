using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalr_chat.Hubs
{
    public class Chat : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("newMessage", "anonymous", message);
        }
    }
}