using Microsoft.AspNetCore.Mvc;
using PortalGate.Models;
using PortalGate.Models.DatabaseContext;
using System;
using System.Linq;

namespace PortalGate.Controllers
{
    public class TransitController : Controller
    {
        private PortalGateDbContext db;

        public TransitController(PortalGateDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public string TransitToUnit(int railroadId, int industryId, int unitId)
        {
            UnitStartPageURI unitStartPage = db.UnitStartPageUries.FirstOrDefault(u =>
                u.Railroad == railroadId &&
                u.Industry == industryId
                && u.Unit == unitId);

            if (unitStartPage != null)
                return unitStartPage.URI;

            return String.Empty;
        }
    }
}
