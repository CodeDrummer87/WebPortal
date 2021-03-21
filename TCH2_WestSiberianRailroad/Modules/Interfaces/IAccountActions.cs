using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;

namespace TCH2_WestSiberianRailroad.Modules.Interfaces
{
    public interface IAccountActions
    {
        User GetCurrentUser();
        string GetUrlUserAccount(int userPositionId);
        string GetViewUserAccount(int userPositionId);
    }
}
