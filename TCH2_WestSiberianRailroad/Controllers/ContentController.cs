using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System.Linq;
using TCH2_WestSiberianRailroad.Models;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private CurrentAppContext db;
        private IAccountActions account;

        public ContentController(CurrentAppContext context, IAccountActions _account)
        {
            db = context;
            account = _account;
        }

        public IActionResult Admin()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult DriverAssistant()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult Contractor()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult Driver()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult DriverInstructor()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult Engineer()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
        }

        public IActionResult HR()
        {
            return account.GetCurrentUser() == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(account.GetCurrentUser());
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
            var result = db.Roles.Where(i => i.IsActual == isActual)
                .GroupJoin(db.Users.Where(u => u.IsActual == isActual).GroupBy(u => u.RoleId).Select(g => new
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
            User user = account.GetCurrentUser();

            if (user != null && user.FirstName != null)
            {
                return true;
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

        [HttpGet]
        public string GetCurrentUserById(int userId)
        {
            var result = db.Users
                .Where(u => u.Id == userId).Select(u => new
                {
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.MiddleName,
                    u.RoleId,
                    u.PositionId
                });

            return JsonConvert.SerializeObject(result);
        }

        [HttpPut]
        public string UpdateEmployeeData(int userId, string email, string firstName, string lastName, string middleName, int positionId, int roleId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Email = email;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.MiddleName = middleName;
                user.PositionId = positionId;
                user.RoleId = roleId;

                db.SaveChanges();
                return $"Данные сотрудника {user.LastName} {user.FirstName[0]}.{user.MiddleName[0]} обновлены";
            }
            return "Сотрудник не найден";
        }

        [HttpDelete]
        public string RemoveEmployee(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.IsActual = 0;
                db.SaveChanges();
                return $"Сотрудник {user.LastName} {user.FirstName[0]}.{user.MiddleName[0]}. перенесён в архив.";
            }

            return "Ошибка удаления: сотрудник не найден";
        }

        [HttpPut]
        public string RecoverEmployeeFromArchive(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.IsActual = 1;
                db.SaveChanges();

                return $"Сотрудник {user.LastName} {user.FirstName[0]}.{user.MiddleName[0]}. восстановлен из архива";
            }

            return string.Empty;
        }

        [HttpPost]
        public string SaveNewPosition([FromBody]NewPositionData data)
        {
            if (data != null)
            {
                Position newPosition = new Position
                {
                    FullName = data.PositionName,
                    Abbreviation = data.Abbreviation,
                    IsActual = 1
                };

                db.Positions.Add(newPosition);
                db.SaveChanges();
            }

            return string.Empty;
        }

        [HttpGet]
        public string GetCurrentPositionById(int positionId)
        {
            var position = db.Positions.Where(p => p.Id == positionId).Select(pos => new 
            {
                FullName = pos.FullName,
                Abbreviation = pos.Abbreviation
            });

            if (position != null)
            {
                return JsonConvert.SerializeObject(position);
            }

            return string.Empty;
        }

        [HttpPut]
        public string UpdatePositionData(int positionId, string positionName, string abbreviation)
        {
            Position position = db.Positions.FirstOrDefault(p => p.Id == positionId);
            if (position != null)
            {
                position.FullName = positionName;
                position.Abbreviation = abbreviation == null ? string.Empty : abbreviation;
                db.SaveChanges();

                return $"Должность {position.FullName} изменена";
            }

            return string.Empty;
        }

        [HttpDelete]
        public string RemovePosition(int positionId)
        {
            Position position = db.Positions.FirstOrDefault(p => p.Id == positionId);
            if (position != null)
            {
                position.IsActual = 0;
                db.SaveChanges();

                return $"Должность {position.FullName} перенесена в архив";
            }

            return string.Empty;
        }
    }
}
