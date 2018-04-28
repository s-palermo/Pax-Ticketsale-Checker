using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaxTicketSaleChecker.Hubs
{
    public class PaxNotificationHubMessageHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Lazy<PaxNotificationHubMessageHelper> _instance = new Lazy<PaxNotificationHubMessageHelper>(
            () => new PaxNotificationHubMessageHelper(GlobalHost.ConnectionManager.GetHubContext<PaxNotificationHub>()));
        public static PaxNotificationHubMessageHelper Instance { get { return _instance.Value; } }

        private static IHubContext _context;

        private PaxNotificationHubMessageHelper(IHubContext context)
        {
            _context = context;
        }

        public void BroadcastPAXTweetAlert(string url)
        {            
            _context.Clients.All.paxTweetReceived(url);
            log.Info("Sent URL:" + url + " to " + PaxNotificationHub.ConnectedIds.Count + ((PaxNotificationHub.ConnectedIds.Count == 1) ? " User!":" Users!"));
        }

        public void BroadcastAnnouncement(string announcement)
        {
            _context.Clients.All.announcement(announcement);
            log.Info("Announcement broadcast:" + announcement);
        }

        public void BroadcastNormalTweet(string user,string message)
        {
            _context.Clients.All.normalTweetReceived(user,message);
        }

        public void BroadcastUnexpectedStreamEvent(string eventType)
        {
            _context.Clients.All.unexpectedStreamEvent(eventType);
            log.Info("Unexpected stream event sent:" + eventType);
        }
    }
}