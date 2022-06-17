using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using SiteCore.Data;

namespace SiteCore.Hubs
{
   
    public class NotificationHub : Hub<IHubClient>
    {

        UserInfoHelper userInfoHelper;
       // private UserInfo _userInfo;
        private UserInfo userInfo => userInfoHelper.GetInfo(Context.User?.Identity.Name);
       
        public NotificationHub(UserInfoHelper userInfoHelper)
        {
            this.userInfoHelper = userInfoHelper;
        }

        public static List<string> GetGroupNamesNewCSListState(string[] CODE_MO, bool isAdmin)
        {
            var result = CODE_MO.Select(x => GetGroupNameNewCSListState(x, false)).ToList();
            if(isAdmin)
                result.Add(GetGroupNameNewCSListState("", true));
            return result;
        }

        public static string GetGroupNameNewCSListState(string CODE_MO, bool isAdmin)
        {
            return isAdmin ? $"CSListState:Admin" : $"CSListState{CODE_MO}";
        }

        public static string GetGroupNameNewPackState(string CODE_MO)
        {
            return $"NewPackState{CODE_MO}";
        }
        public async Task RegisterNewCSListState()
        {
            var CODE_MO = userInfo.CODE_MO;
            var isAdmin = Context.User.IsInRole("Admin");
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameNewCSListState(CODE_MO, isAdmin));
        }



        public async Task RegisterNewPackState()
        {
            var CODE_MO = userInfo.CODE_MO;
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupNameNewPackState(CODE_MO));
        }
    }

    public interface IHubClient
    {
        Task NewCSListState(List<int> ID);
        Task NewPackState();
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
