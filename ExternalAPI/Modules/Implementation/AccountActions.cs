using ExternalAPI.Modules.Interfaces;

namespace ExternalAPI.Modules.Implementation
{
    public class AccountActions : IAccountActions
    {
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
