using RailroadPortalClassLibrary;

namespace ExternalAPI.Modules.Interfaces
{
    public interface IAccountActions
    {
        string GetUrlUserAccount(int userPositionId);
        string GetViewUserAccount(int userPositionId);
    }
}
