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
        public async Task Register()
        {
            var CODE_MO =  Context.User?.CODE_MO();
            if (!string.IsNullOrEmpty(CODE_MO))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, CODE_MO);
            
            }
        }
    }
}
