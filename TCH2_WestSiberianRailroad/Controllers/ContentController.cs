using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System.Linq;
using System.Threading.Tasks;
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

        [AllowAnonymous]
        public IActionResult ConfirmedAccount(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.ConfirmedEmail = 1;
                db.SaveChangesAsync();
            }
            return user == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(user);
        }

        [AllowAnonymous]
        public IActionResult UnconfirmedAccount()
        {
            return View();
        }

        [HttpGet]
        public string GetEmployees(int page)
        {
            var employeesList = db.Users.Join(db.Positions, u => u.PositionId, p => p.Id, (u, p) => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.MiddleName,
                p.FullName,
                u.Email,
                u.ConfirmedEmail
            }).OrderBy(e => e.LastName)
            .Skip(page * 14).Take(14);

            return JsonConvert.SerializeObject(employeesList);
        }

        [HttpGet]
        public string GetPositions(int page)
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
                }).Skip(page * 14).Take(14).ToList();

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string GetRoles(int page)
        {
            var result = db.Roles.GroupJoin(db.Users.GroupBy(u => u.RoleId).Select(g => new
            {
                RoleId = g.Key,
                Count = g.Count()
            }),
                        r => r.Id,
                        u => u.RoleId,
                        (r, u) => new { r, u })
                .SelectMany(all => all.u.DefaultIfEmpty(), (role, user) => new
                {
                    Id = role.r.Id,
                    RoleName = role.r.Name,
                    Count = user.Count
                }).Skip(page * 14).Take(14).ToList();

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

        [HttpGet]
        public int GetEmployeeCount()
        {
            return db.Users.Count();
        }

        [HttpGet]
        public int GetPositionCount()
        {
            return db.Positions.Count();
        }

        [HttpGet]
        public int GetRoleCount()
        {
            return db.Roles.Count();
        }

        [HttpGet]
        public int GetEmailCount()
        {
            return db.AppEmailAccounts.Count();
        }

        [HttpGet]
        public string GetAppEmailAccount(int page)
        {
            var result = db.AppEmailAccounts.Select(e => new 
            {
                e.Id,
                e.Email,
                e.IsActual
            }).Skip(page * 14).Take(14);
            return JsonConvert.SerializeObject(result);
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
