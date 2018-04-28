# Pax-Ticketsale-Checker

MVC Web App That Receives Filtered Twitter Stream from @PAX_Badges and sends SignalR Broadcast to Clients

## About

This ASP.NET MVC site is designed to listen for tweets from @PAX_Badges.  This is in order to broadcast a message to all listening clients
that the tickets have gone on sale.

Once a tweet has been sent out that meets the checks for being a ticket sale announcement the broadcast will be sent out to all clients
with the URLs contained in that tweet to purchase tickets and hotels.

### Website

http://spc-g.dynamic-dns.net:8088

### Client

This site was build with an accompanying <a href="https://github.com/s-palermo/Pax-Ticketsite-AutoLauncher">Chrome Extension Client</a>


#### Note

This server is not always running.
If it is comming close to a sale date and there is no link for the site or it is down please open an issue.