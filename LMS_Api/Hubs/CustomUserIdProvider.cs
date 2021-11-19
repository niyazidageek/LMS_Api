using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Business.Concrete
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            var a = connection.User?.FindFirst(ClaimTypes.Email)?.Value;
            return connection.User?.Claims?.FirstOrDefault(c => c.Type == "uid")?.Value;
        }
    }
}
