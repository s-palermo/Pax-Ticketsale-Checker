using PaxTicketSaleChecker.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi.Events;
using Tweetinvi.Models.Entities;

namespace PaxTicketSaleChecker.TwitterAPI
{
    public class StreamEventCallbackManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void FilteredStream_MatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs e)
        {
            log.Info("TWEET HAS BEEN POSTED BY " + e.Tweet.CreatedBy.ScreenName);            
            //matches tweets @ followed users so we need to parse those out here
            if ((e.Tweet.CreatedBy.ScreenName == "PAX_Badges" ||
                e.Tweet.CreatedBy.ScreenName == "PaxAutoQueuer")
                && (String.IsNullOrEmpty(e.Tweet.InReplyToScreenName)))
            {
                log.Info("OFFICIAL TWEET**********************************");
                List <String> foundUrls = new List<String>();
                foreach (IUrlEntity url in e.Tweet.Entities.Urls)
                {
                    if (!foundUrls.Contains(url.ExpandedURL))
                    {
                        log.Info("*********************************URL FOUND*********************************");
                        log.Info("URL:"+ url.ExpandedURL);
                        PaxNotificationHubMessageHelper.Instance.BroadcastPAXTweetAlert(url.ExpandedURL);
                        foundUrls.Add(url.ExpandedURL);
                    }
                }
            }
            log.Info(e.Tweet.FullText);
            PaxNotificationHubMessageHelper.Instance.BroadcastNormalTweet(e.Tweet.CreatedBy.ScreenName, e.Tweet.FullText);
        }
        
        public static void Stream_Stopped(object sender, StreamExceptionEventArgs e)
        {
            log.Error("Stream Stopped Unexpectedly.", e.Exception);
            log.Info("Attempting to restart stream."); 
            TwitterAPI.ConnectionManager.RegisterFilteredStreamEventsAsync(new string[] { "PAX_Badges", "PaxAutoQueuer" }, null);
            PaxNotificationHubMessageHelper.Instance.BroadcastUnexpectedStreamEvent("STREAM STOPPED");
        }

        public static void Stream_Disconnected(object sender, DisconnectedEventArgs e)
        {
            log.Error("Stream Disconnected Unexpectedly. Reason:" + e.DisconnectMessage.Reason);
            log.Info("Attempting to restart stream.");
            TwitterAPI.ConnectionManager.RegisterFilteredStreamEventsAsync(new string[] { "PAX_Badges", "PaxAutoQueuer" }, null);
            PaxNotificationHubMessageHelper.Instance.BroadcastUnexpectedStreamEvent("STREAM DISCONNECTED");
        }

        public static void Stream_Paused(object sender, EventArgs e)
        {
            log.Error("STREAM PAUSED.  Reason:" + e.ToString());
            PaxNotificationHubMessageHelper.Instance.BroadcastUnexpectedStreamEvent("STREAM PAUSED");
        }

        public static void Stream_Resumed(object sender, EventArgs e)
        {
            log.Info("STREAM RESUMED.  Reason:" + e.ToString());
            PaxNotificationHubMessageHelper.Instance.BroadcastUnexpectedStreamEvent("STREAM RESUMED");
        }
    }
}