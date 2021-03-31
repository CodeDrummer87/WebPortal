using PortalGate.Models.DatabaseContext;
using PortalGate.Modules.Interfaces;
using System.Linq;

namespace PortalGate.Modules.Implementation
{
    public class Transfer : ITransfer
    {
        private PortalGateDbContext db;

        public Transfer(PortalGateDbContext context)
        {
            db = context;
        }

        public IQueryable GetRailroadList()
        {
            return db.RailroadList.Select(railroad => new
            {
                railroad.Id,
                railroad.FullTitle
            });
        }

        public IQueryable GetUnitList(int railroadId, int industryId, int page)
        {
            int unitsCount = db.Units.Where(u => u.Railroad == railroadId && u.Industry == industryId).Count();

            IQueryable list;

            if (unitsCount > 12)
            {
                list = db.Units.
                    Where(u => u.Railroad == railroadId && u.Industry == industryId).
                    Select(u => new
                    {
                        u.Id,
                        u.ShortTitle
                    }).Skip(page * 12).Take(12);
            }
            else
            {
                list = db.Units.Where(u => u.Railroad == railroadId && u.Industry == industryId).Select(u => new
                {
                    u.Id,
                    u.ShortTitle
                });
            }

            return list;
        }

        public IQueryable GetIndustryList()
        {
            return db.Industries.Select(i => new
            {
                i.Id,
                i.Abbreviation
            });
        }

        public int GetUnitsCount(int railroadId, int industryId)
        {
            return db.Units.Where(u => u.Railroad == railroadId && u.Industry == industryId).Count();
        }
    }
}
