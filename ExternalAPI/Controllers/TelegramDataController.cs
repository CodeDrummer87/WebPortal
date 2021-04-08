using ExternalAPI.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// Telegram controller for GET/POST methods for managing data of telegrams
    /// </summary>
    [Route("api/telegramdata")]
    [ApiController]
    public class TelegramDataController : ControllerBase
    {
        private CurrentAppContext db;

        public TelegramDataController(CurrentAppContext context)
        {
            db = context;
        }

        /// <summary>
        /// This POST method creates a new telegram in app 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Response message</returns>
        [Route("createNewTelegram")]
        [HttpPost]
        public async Task<string> Post([FromBody] TelegramModel model)
        {
            string response = string.Empty;

            await Task.Run(() =>
            {
                if (model != null)
                {
                    Telegram telegram = new Telegram
                    {
                        Created = DateTime.Today,
                        Subject = model.Subject,
                        Content = model.Content,
                        IsActual = 1
                    };

                    db.LaborProtectionTelegrams.Add(telegram);
                    db.SaveChanges();

                    response = "Телеграмма успешно сохранена";
                }
            });

            return response;
        }

        /// <summary>
        /// This GET method returns a list of saved telegrams
        /// </summary>
        /// <returns>List of telegrams</returns>
        [Route("getTelegramsList")]
        [HttpGet]
        public async Task<string> Get()
        {
            string response = string.Empty;

            await Task.Run(() =>
            {
                var list = db.LaborProtectionTelegrams.Select(t => new
                {
                    Id = t.Id,
                    Created = t.Created.Date,
                    Subject = t.Subject,
                    IsActual = t.IsActual
                }).ToList();
                response = JsonConvert.SerializeObject(list);
            });

            return response;
        }
    }
}
