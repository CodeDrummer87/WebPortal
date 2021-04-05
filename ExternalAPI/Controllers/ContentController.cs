using ExternalAPI.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System.Linq;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// Content сontroller for GET/POST/PUT/DELETE for administering system entities
    /// </summary>
    [Route("api/content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private CurrentAppContext db;

        public ContentController(CurrentAppContext context)
        {
            db = context;
        }

        /// <summary>
        /// This GET method allows to get entities depending on entityType
        /// </summary>
        /// <param name="page"></param>
        /// <param name="isActual"></param>
        /// <param name="entityType"></param>
        /// <returns>Entity collection for page 'n'</returns>
        [Route("getEntity")]
        [HttpGet]
        public string Get(int page, byte isActual, string entityType)
        {
            string response = string.Empty;

            switch(entityType)
            {
                case "employees":
                    response = GetEmployees(page, isActual);
                    break;

                case "positions":
                    response = GetPositions(page, isActual);
                    break;

                case "roles":
                    response = GetRoles(page, isActual);
                    break;
            }

            return response;
        }

        /// <summary>
        /// This GET method allows to get a count of entity depending on entityType
        /// </summary>
        /// <param name="isActual"></param>
        /// <param name="entity"></param>
        /// <returns>Entity count in JSON</returns>
        [Route("getCount")]
        [HttpGet]
        public string Get(byte isActual, string entity)
        {
            string response = string.Empty;
            int count = 0;

            switch (entity)
            {
                case "users":
                    count = db.Users.Where(u => u.IsActual == isActual).Count();
                    break;
                case "positions":
                    count = db.Positions.Where(p => p.IsActual == isActual).Count();
                    break;
                case "roles":
                    count = db.Roles.Where(r => r.IsActual == isActual).Count();
                    break;
                case "emails":
                    count = db.AppEmailAccounts.Count();
                    break;
            }

            response = count.ToString();
            return response;
        }

        /// <summary>
        /// This GET method allows to get current email of application
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Current email of application</returns>
        [Route("getAppEmailAccount")]
        [HttpGet]
        public string Get(int page)
        {
            var result = db.AppEmailAccounts.Select(e => new
            {
                e.Id,
                e.Email,
                e.IsActual
            }).Skip(page * 14).Take(14);

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// This GET method allows to get data of entity depending on entityType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [Route("getDataById")]
        [HttpGet]
        public string Get(int id, string dataType)
        {
            string result = string.Empty;

            switch (dataType)
            {
                case "user":
                    var user = db.Users
                    .Where(u => u.Id == id).Select(u => new
                    {
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        u.MiddleName,
                        u.RoleId,
                        u.PositionId
                    });

                    result = JsonConvert.SerializeObject(user);
                    break;
                case "position":
                    var position = db.Positions.Where(p => p.Id == id).Select(pos => new
                    {
                        FullName = pos.FullName,
                        Abbreviation = pos.Abbreviation
                    });

                    if (position != null)
                    {
                        result = JsonConvert.SerializeObject(position);
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// This PUT method updates employee data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        /// <param name="positionId"></param>
        /// <param name="roleId"></param>
        /// <returns>Response message</returns>
        [Route("updateEmployeeData")]
        [HttpPut]
        public string Put(int userId, string email, string firstName, string lastName, string middleName, int positionId, int roleId)
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

        /// <summary>
        /// This DELETE method removes entity data depending on entityType 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataType"></param>
        /// <returns>Response message</returns>
        [Route("removeDataById")]
        [HttpDelete]
        public string Delete(int id, string dataType)
        {
            string responseMessage = string.Empty;

            switch (dataType)
            {
                case "user":
                    User user = db.Users.FirstOrDefault(u => u.Id == id);
                    if (user != null)
                    {
                        user.IsActual = 0;
                        db.SaveChanges();
                        return $"Сотрудник {user.LastName} {user.FirstName[0]}.{user.MiddleName[0]}. перенесён в архив.";
                    }

                    responseMessage = "Ошибка удаления: сотрудник не найден";
                    break;

                case "position":
                    Position position = db.Positions.FirstOrDefault(p => p.Id == id);
                    if (position != null)
                    {
                        position.IsActual = 0;
                        db.SaveChanges();

                        return $"Должность {position.FullName} перенесена в архив";
                    }
                    break;
            }

            return responseMessage;
        }

        /// <summary>
        /// This PUT method recovers entity data by id depending on entityType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataType"></param>
        /// <returns>Response message</returns>
        [Route("recoverDataById")]
        [HttpPut]
        public string Put(int id, string dataType)
        {
            string responseMessage = string.Empty;

            switch(dataType)
            {
                case "user":
                    User user = db.Users.FirstOrDefault(u => u.Id == id);
                    if (user != null)
                    {
                        user.IsActual = 1;
                        db.SaveChanges();

                        responseMessage = $"Сотрудник {user.LastName} {user.FirstName[0]}.{user.MiddleName[0]}. восстановлен из архива";
                    }

                    break;

                case "position":
                    Position position = db.Positions.FirstOrDefault(p => p.Id == id);
                    if (position != null)
                    {
                        position.IsActual = 1;
                        db.SaveChanges();

                        responseMessage = $"Должность {position.FullName} восстановлена из архива";
                    }

                    break;
            }

            return responseMessage;
        }

        /// <summary>
        /// This PUT method updates position data
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="positionName"></param>
        /// <param name="abbreviation"></param>
        /// <returns>Response message</returns>
        [Route("updatePositionData")]
        [HttpPut]
        public string Put(int positionId, string positionName, string abbreviation)
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

        /// <summary>
        /// This POST method creates a new position
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("saveNewPosition")]
        [HttpPost]
        public string Post(NewPositionData data)
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

                return $"Должность {newPosition.FullName} создана";
            }

            return string.Empty;
        }

        private string GetEmployees(int page, byte isActual)
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

        private string GetPositions(int page, byte isActual)
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

        private string GetRoles(int page, byte isActual)
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
    }
}
