using Microsoft.AspNet.SignalR;

namespace ChildWatchEmployee
{
    public class ChildWatchHub : Hub
    {
        public void SendRefreshNotification()
        {
            Clients.Others.UpdateChildTable();
        }     
    }
}