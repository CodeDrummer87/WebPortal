using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System.Linq;
using TCH2_WestSiberianRailroad.Models;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private CurrentAppContext db;

        public ContentController(CurrentAppContext context)
        {
            db = context;
        }

        public IActionResult Admin()
        {
            return View();
        }


        [HttpGet]
        public string GetEmployees()
        {
            var result = db.Users.Select(u => new 
            {
                u.Id,
                u.Email
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string GetPositions()
        {
            var result = db.Positions.Select(p => new 
            {
                p.Id,
                p.FullName
            });

            return JsonConvert.SerializeObject(result);
        }
    }
}
