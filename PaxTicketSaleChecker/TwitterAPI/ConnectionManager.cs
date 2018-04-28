using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace PaxTicketSaleChecker.TwitterAPI
{
    public class ConnectionManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string access_token;
        private static string access_token_secret;
        private static string consumer_key;
        private static string consumer_secret;
        private static IFilteredStream stream;
        public static void Init()
        {
            access_token = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("TwitterAPIAccessToken");
            access_token_secret = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("TwitterAPIAccessTokenSecret");
            consumer_key = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("TwitterAPIConsumerKey");
            consumer_secret = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("TwitterAPIConsumerSecret");
            Authenticate();
        }

        private static void Authenticate()
        {
            try
            {
                Auth.SetUserCredentials(consumer_key, consumer_secret, access_token,
                access_token_secret);
            }
            catch(Exception e)
            {
                log.Error("Twitter API Authentication Failed.", e);
            }
            
        }

        public static async Task RegisterFilteredStreamEventsAsync(string[] users,string[] trackKeywords)
        {
            if(users == null)
            {
                log.Fatal("No Users Are Being Followed.  Server is useless.");
                return;
            }

            try
            {                
                stream = Stream.CreateFilteredStream();
                foreach (string userName in users)
                {
                    IUserIdentifier myUser = Tweetinvi.User.GetUserFromScreenName(userName);
                    stream.AddFollow(myUser);
                }
                if (trackKeywords != null)
                {
                    foreach (string keyword in trackKeywords)
                    {
                        stream.AddTrack(keyword);
                    }
                }
                stream.MatchingTweetReceived += StreamEventCallbackManager.FilteredStream_MatchingTweetReceived;
                stream.StreamStopped += StreamEventCallbackManager.Stream_Stopped;
                stream.DisconnectMessageReceived += StreamEventCallbackManager.Stream_Disconnected;
                stream.StreamPaused += StreamEventCallbackManager.Stream_Paused;
                stream.StreamResumed += StreamEventCallbackManager.Stream_Resumed;
                await stream.StartStreamMatchingAllConditionsAsync();
                log.Info("Now Listening To Stream Events.");
            }
            catch(Exception e)
            {
                log.Fatal("FAILED TO REGISTER TO STREAM EVENTS!", e);
            }
        }

        public static void UnRegisterFilteredStreamEvents()
        {
            try
            {
                stream.StopStream();
            }
            catch(Exception e)
            {
                log.Error("Unable to stop stream.", e);
            }
        }
    }
}