using Microsoft.AspNetCore.Mvc;
using PortalGate.Modules.Interfaces;

namespace PortalGate.Controllers
{
    public class TransitController : Controller
    {
        private readonly ITransitData transitData;

        public TransitController(ITransitData _transit)
        {
            transitData = _transit;
        }

        [HttpGet]
        public string TransitToUnit(int railroadId, int industryId, int unitId)
        {
            return transitData.TransitToUnit(railroadId, industryId, unitId);
        }
    }
}
