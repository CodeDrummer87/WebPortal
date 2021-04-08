using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Modules.Implementation
{
    public class AccountActions : IAccountActions
    {
        private readonly IHttpContextAccessor contextAccessor;
        private TCH2_WebClient webClient;

        public AccountActions(IHttpContextAccessor accessor)
        {
            contextAccessor = accessor;
            webClient = new TCH2_WebClient();
        }

        public User GetCurrentUser()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            if (sessionId != null)
            {
                string userAsString = webClient.Get("api/account/getCurrentUser", "?sessionId=" + sessionId);
                if (userAsString != string.Empty)
                {
                    return JsonConvert.DeserializeObject<User>(userAsString);
                }
            }

            return null;
        }

        public string GetUrlUserAccount(int userPositionId)
        {
            switch (userPositionId)
            {
                case 1: return "/Content/Admin";
                case 2: return "/Content/HR";
                case 3: return "/Content/Contractor";
                case 5: return "/Content/Driver";
                case 6: return "/Content/DriverInstructor";
                case 7: return "/Content/Engineer";
                default: return "/Content/DriverAssistant";
            }
        }

        public string GetViewUserAccount(int userPositionId)
        {
            switch (userPositionId)
            {
                case 1: return "Admin";
                case 2: return "HR";
                case 3: return "Contractor"; 
                case 5: return "Driver";
                case 6: return "DriverInstructor";
                case 7: return "Engineer";
                default: return "DriverAssistant";
            }
        }
    }
}
