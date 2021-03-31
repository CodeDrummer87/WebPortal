using PortalGate.Models;
using PortalGate.Models.DatabaseContext;
using PortalGate.Modules.Interfaces;
using System;
using System.Linq;

namespace PortalGate.Modules.Implementation
{
    public class TransitData : ITransitData
    {
        private PortalGateDbContext db;

        public TransitData(PortalGateDbContext context)
        {
            db = context;
        }

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
