using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BackEnd.Hubs
{
    public class ViewHub: Hub
    {
        public static int ViewCount { get; set; } = 0;

        public async Task NotifyWatching()
        {
            ViewCount++;

            await Clients.All.SendAsync("viewCountUpdate", ViewCount);
        }
    }
}