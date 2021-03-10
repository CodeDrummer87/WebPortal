using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private IHttpContextAccessor contextAccessor;

        public ContentController(CurrentAppContext context, IHttpContextAccessor accessor)
        {
            db = context;
            contextAccessor = accessor;
        }

        public IActionResult Admin()
        {
            return GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(GetCurrentUser());
        }

        [HttpGet]
        public string GetEmployees()
        {
            var result = db.Users.Join(db.Positions, u => u.PositionId, p => p.Id, (u, p) => new
            { 
                u.Id,
                u.FirstName,
                u.LastName,
                u.MiddleName,
                p.FullName,
                u.Email
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string GetPositions()
        {
            var result = db.Positions.GroupJoin(db.Users.GroupBy(u => u.PositionId).Select(g => new
            {
                PositionId = g.Key,
                Count = g.Count()
            }),
                        p => p.Id,
                        u => u.PositionId,
                        (p, u) => new { p, u })
                .SelectMany(all => all.u.DefaultIfEmpty(), (position, user) => new
                {
                    Id = position.p.Id,
                    FullName = position.p.FullName,
                    Count = user.Count
                }).ToList();

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public bool CheckForName()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            if (sessionId != null)
            {
                int userId = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId).UserId;
                User user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && user.FirstName != null)
                {
                    return true;
                }
            }

            return false;
        }

        private User GetCurrentUser()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            if (sessionId != null)
            {
                int userId = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId).UserId;
                User user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
