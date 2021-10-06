using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using SiteCore.Data;

namespace SiteCore.Hubs
{
   
    public class NotificationHub : Hub
    {
        UserInfoHelper userInfoHelper;
        private UserInfo _userInfo;
        private UserInfo userInfo
        {
            get
            {
                return userInfoHelper.GetInfo(Context.User?.Identity.Name);
            }
        }

        public NotificationHub(UserInfoHelper userInfoHelper)
        {
            this.userInfoHelper = userInfoHelper;
        }
        public async Task Register()
        {
            var CODE_MO = userInfo.CODE_MO;
            if (!string.IsNullOrEmpty(CODE_MO))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, CODE_MO);
            }
        
            if(Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
        }
        public async Task RegisterForLoadThemeFile()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);
        }


    }


    public static class ExtNotificationHub
    {
        public static void Progress(this IHubContext<NotificationHub> hub, string ConnectionId, int process, int maxProcess, string message)
        {
            if(!string.IsNullOrEmpty(ConnectionId))
                hub.Clients.Groups(ConnectionId).SendAsync(nameof(Progress), new { process, maxProcess, message });
        }
    }
  
}
