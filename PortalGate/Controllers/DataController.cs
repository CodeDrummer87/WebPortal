using Microsoft.AspNetCore.Mvc;
using PortalGate.Modules.Interfaces;
using System.Linq;

namespace PortalGate.Controllers
{
    public class DataController : Controller
    {
        private readonly ITransfer transfer;

        public DataController(ITransfer _transfer)
        {
            transfer = _transfer;
        }

        [HttpGet]
        public IQueryable GetRailroadList()
        {
            return transfer.GetRailroadList();
        }

        [HttpGet]
        public IQueryable GetUnitList(int railroadId, int industryId, int page)
        {
            return transfer.GetUnitList(railroadId, industryId, page);
        }

        [HttpGet]
        public IQueryable GetIndustryList()
        {
            return transfer.GetIndustryList();
        }


        [HttpGet]
        public int GetUnitsCount(int railroadId, int industryId)
        {
            return transfer.GetUnitsCount(railroadId, industryId);
        }
    }
}
