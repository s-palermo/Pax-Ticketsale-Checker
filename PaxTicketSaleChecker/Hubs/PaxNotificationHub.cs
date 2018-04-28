using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace PaxTicketSaleChecker.Hubs
{
    public class PaxNotificationHub : Hub
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static HashSet<string> ConnectedIds = new HashSet<string>();

        public override Task OnConnected()
        {
            PaxNotificationHub.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            PaxNotificationHub.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            PaxNotificationHub.ConnectedIds.Add(Context.ConnectionId);
            return base.OnReconnected();
        }
    }
}