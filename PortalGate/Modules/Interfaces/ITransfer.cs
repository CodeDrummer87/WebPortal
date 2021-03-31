using System.Linq;

namespace PortalGate.Modules.Interfaces
{
    public interface ITransfer
    {
        IQueryable GetRailroadList();
        IQueryable GetUnitList(int railroadId, int industryId, int page);
        IQueryable GetIndustryList();
        int GetUnitsCount(int railroadId, int industryId);
    }
}
