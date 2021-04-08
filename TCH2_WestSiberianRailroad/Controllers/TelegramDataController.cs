using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TCH2_WestSiberianRailroad.Modules.Implementation;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class TelegramDataController : Controller
    {
        private TCH2_WebClient webClient;

        public TelegramDataController()
        {
            webClient = new TCH2_WebClient();
        }

        [HttpPost]
        public async Task<string> CreateNewTelegram([FromBody] TelegramModel model)
        {
            string response = String.Empty;

            await Task.Run(() =>
            {
                if (model != null)
                {
                    if (CheckForTelegramContent(model))
                    {
                        response = webClient.Send<TelegramModel>(HttpMethod.Post, "api/telegramdata/createnewtelegram", model);
                    }
                    else
                    {
                        response = "Ответ сервера: телеграмма должна иметь тему и содержание";
                    }
                }
            });            

            return response;
        }

        [HttpGet]
        public async Task<string> GetTelegramsList()
        {
            string response = String.Empty;

            await Task.Run(() =>
            {
                response = webClient.Get("api/telegramdata/getTelegramsList", "");
            });

            return response;
        }

        private bool CheckForTelegramContent(TelegramModel model)
        {
            return (model.Subject != String.Empty && model.Content != String.Empty) ? true : false;
        }
    }
}
