using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;
using System.Linq;
using TCH2_WestSiberianRailroad.Models;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Modules.Implementation
{
    public class AccountActions : IAccountActions
    {

        private CurrentAppContext db;
        private readonly IHttpContextAccessor contextAccessor;

        public AccountActions(CurrentAppContext context, IHttpContextAccessor accessor)
        {
            db = context;
            contextAccessor = accessor;
        }

        public User GetCurrentUser()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            if (sessionId != null)
            {
                int userId = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId).UserId;
                User user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }

        public string GetUrlUserAccount(int userPositionId)
        {
            switch (userPositionId)
            {
                case 2: return "/Content/HR";
                case 3: return "/Content/Contractor";
                case 4: return "/Content/DriverAssistant";
                case 5: return "/Content/Driver";
                case 6: return "/Content/DriverInstructor";
                case 7: return "/Content/Engineer";
                default: return "/Content/Admin";
            }
        }

        public string GetViewUserAccount(int userPositionId)
        {
            switch (userPositionId)
            {
                case 2: return "HR";
                case 3: return "Contractor";
                case 4: return "DriverAssistant";
                case 5: return "Driver";
                case 6: return "DriverInstructor";
                case 7: return "Engineer";
                default: return "Admin";
            }
        }
    }
}
