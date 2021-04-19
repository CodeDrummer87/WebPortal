using ExternalAPI.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// ColumnData controller for GET/POST methods
    /// </summary>
    [Route("api/columnData")]
    [ApiController]
    public class ColumnDataController : ControllerBase
    {
        private CurrentAppContext db;

        public ColumnDataController(CurrentAppContext context)
        {
            db = context;
        }

        /// <summary>
        /// This GET method allows to get list of columns
        /// </summary>
        /// <param name="page"></param>
        /// <param name="isActual"></param>
        /// <returns>Columns collection for page 'n'</returns>
        [Route("getColumnsList")]
        [HttpGet]
        public string Get(int page, byte isActual)
        {
            var columns = db.Users.Where(u => u.IsActual == isActual).Join(db.Columns, u => u.Id, c => c.Trainer, (u, c) => new
            {
                Id = c.Id,
                Trainer = $"{u.LastName} {u.FirstName[0]}.{u.MiddleName[0]}.",
                SpecializationId = c.Specialization
            }).Join(db.ColumnTypes, uc => uc.SpecializationId, ct => ct.Id, (uc, ct) => new
            {
                Id = uc.Id,
                Trainer = uc.Trainer,
                Specialization = ct.Name
            }).GroupJoin(db.ColumnStaff.GroupBy(cs => cs.ColumnId).Select(g => new
            {
                ColumnId = g.Key,
                Count = g.Count()
            }),
                a => a.Id,
                cs => cs.ColumnId,
                (a, cs) => new { a, cs })
            .SelectMany(x => x.cs.DefaultIfEmpty(), (all, cs) => new
            {
                Id = all.a.Id,
                Specialization = all.a.Specialization,
                Trainer = all.a.Trainer,
                Total = cs.Count
            }).Skip(page * 14).Take(14);

            return JsonConvert.SerializeObject(columns);
        }

        /// <summary>
        /// This GET method returns the number of records in the Columns table 
        /// </summary>
        /// <param name="isActual"></param>
        /// <returns>Number of records in the Columns table</returns>
        [Route("getColumnsCount")]
        [HttpGet]
        public string Get(byte isActual)
        {
            return db.Columns.Where(c => c.IsActual == isActual).Count().ToString();
        }

        /// <summary>
        /// This GET method returns a list of column specializations
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("getColumnSpecialization")]
        [HttpGet]
        public string Get(int page)
        {
            var response = db.ColumnTypes.Skip(page * 14).Take(14).ToList();
            return JsonConvert.SerializeObject(response);
        }

        [Route("getCount")]
        [HttpGet]
        public string Get(string entity)
        {
            switch (entity)
            {
                case "drivers":
                    var result = db.Columns.GroupJoin(db.ColumnStaff
                        .Join(db.Users.Where(u => u.PositionId == 5), cs=>cs.UserId, u=>u.Id, (cs, u) => new 
                        {
                            ColumnId = cs.ColumnId,
                            userId = u.Id
                        })
                        .GroupBy(cs => cs.ColumnId)
                        .Select(g => new
                        {
                            ColumnId = g.Key,
                            Count = g.Count()
                        }),
                            c => c.Id,
                            cs => cs.ColumnId,
                            (c, cs) => new { c, cs })
                        .SelectMany(x => x.cs.DefaultIfEmpty(), (all, cs) => new
                        { 
                            Id = all.c.Id,
                            Count = cs.Count
                        });

                    return JsonConvert.SerializeObject(result);
                default: return string.Empty;
            }
        }

        /// <summary>
        /// This GET method returns a list of driver trainers not attached to columns
        /// </summary>
        /// <returns></returns>
        [Route("getTrainersListForNewColumn")]
        [HttpGet]
        public string Get()
        {
            var freeInstructors = db.Users.Where(a => a.PositionId == 6)
                .Select(u => u.Id).Except(db.Columns.Select(c => c.Trainer));

            var list = db.Users.Join(freeInstructors, u => u.Id, f => f, (u, f) => new
            { 
                u.Id,
                u.LastName,
                u.FirstName,
                u.MiddleName
            });

            return JsonConvert.SerializeObject(list);
        }

        [Route("createNewColumn")]
        [HttpPost]
        public async Task<string> Post([FromBody] NewColumnData data)
        {
            string message = string.Empty;

            await Task.Run(() =>
            {
                Column column = new Column 
                {
                    Specialization = data.Specialization,
                    Trainer = data.Trainer,
                    IsActual = 1
                };

                db.Columns.Add(column);
                db.SaveChanges();

                message = "Колонна успешно создана";
            });

            return message;
        }
    }
}
