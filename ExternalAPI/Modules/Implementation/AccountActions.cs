using ExternalAPI.DatabaseContext;
using ExternalAPI.Modules.Interfaces;

namespace ExternalAPI.Modules.Implementation
{
    public class AccountActions : IAccountActions
    {

        private CurrentAppContext db;

        public AccountActions(CurrentAppContext context)
        {
            db = context;
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
