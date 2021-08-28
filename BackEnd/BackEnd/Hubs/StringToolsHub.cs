using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BackEnd.Hubs
{
    public class StringToolsHub: Hub
    {
        public static string FirstName { get; set; } = "";
        public static string LastName { get; set; } = "";

        public string GetFullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            return $"{FirstName} {LastName}";
        }

        public async Task NotifyFullName()
        {
            await Clients.All.SendAsync("viewFullNameUpdate", $"{FirstName} {LastName}");
        }
    }
}