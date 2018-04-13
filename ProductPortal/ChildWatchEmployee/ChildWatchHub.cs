using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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