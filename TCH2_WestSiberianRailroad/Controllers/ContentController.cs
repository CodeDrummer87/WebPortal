﻿using Microsoft.AspNetCore.Authorization;
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
        public string GetEmployees(int page, byte isActual)
        {
            var employeesList = db.Users.Where(i => i.IsActual == isActual).Join(db.Positions, u => u.PositionId, p => p.Id, (u, p) => new
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
        public string GetPositions(int page, byte isActual)
        {
            var result = db.Positions.Where(i => i.IsActual == isActual).GroupJoin(db.Users.GroupBy(u => u.PositionId).Select(g => new
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
        public string GetRoles(int page, byte isActual)
        {
            var result = db.Roles.Where(i => i.IsActual == isActual).GroupJoin(db.Users.GroupBy(u => u.RoleId).Select(g => new
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
        public int GetEmployeeCount(byte isActual)
        {
            return db.Users.Where(u => u.IsActual == isActual).Count();
        }

        [HttpGet]
        public int GetPositionCount(byte isActual)
        {
            return db.Positions.Where(p => p.IsActual == isActual).Count();
        }

        [HttpGet]
        public int GetRoleCount(byte isActual)
        {
            return db.Roles.Where(r => r.IsActual == isActual).Count();
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
