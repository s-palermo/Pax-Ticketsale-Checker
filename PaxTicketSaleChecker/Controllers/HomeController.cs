using PaxTicketSaleChecker.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Models;

namespace PaxTicketSaleChecker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        
        public ActionResult ConnectionCount()
        {
            return Content(PaxNotificationHub.ConnectedIds.Count.ToString() + ((PaxNotificationHub.ConnectedIds.Count == 1) ? " Connected User!" : " Connected Users!"));
        }

        /*
        public ActionResult BroadcastAnnouncement(string announcement,string apiKey)
        {
            if(apiKey!="ApiKeyAbc123")
            {
                return new HttpUnauthorizedResult();
            }
            PaxNotificationHubMessageHelper.Instance.BroadcastAnnouncement(announcement);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        */
    }
}
