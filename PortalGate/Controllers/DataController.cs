using Microsoft.AspNetCore.Mvc;
using PortalGate.Models.DatabaseContext;
using System.Linq;

namespace PortalGate.Controllers
{
    public class DataController : Controller
    {
        private readonly PortalGateDbContext db;

        public DataController(PortalGateDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IQueryable GetRailroadList()
        {
            return db.RailroadList.Select(railroad => new
            {
                railroad.Id,
                railroad.FullTitle
            });
        }

        [HttpGet]
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

        [HttpGet]
        public IQueryable GetIndustryList()
        {
            return db.Industries.Select(i => new
            {
                i.Id,
                i.Abbreviation
            });
        }


        [HttpGet]
        public int GetUnitsCount(int railroadId, int industryId)
        {
            return db.Units.Where(u => u.Railroad == railroadId && u.Industry == industryId).Count();
        }
    }
}
