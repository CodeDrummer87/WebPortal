using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Controllers
{
    public class ColumnDataController : Controller
    {
        private ITCH2_WebClient webClient;

        public ColumnDataController(ITCH2_WebClient _webClient)
        {
            webClient = _webClient;
        }

        [HttpGet]
        public string GetColumnsList(int page, byte isActual)
        {
            var response = webClient.Get("api/columnData/getColumnsList", "?page=" + page + "&isActual=" + isActual);

            return response;
        }

        [HttpGet]
        public int GetColumnsCount(byte isActual)
        {
            var response = webClient.Get("api/columnData/getColumnsCount", "?isActual=" + isActual);
            return Convert.ToInt32(response);
        }

        [HttpGet]
        public string GetColumnSpecialization(int page)
        {
            return webClient.Get("api/columnData/getColumnSpecialization", "?page=" + page);
        }

        [HttpGet]
        public string GetTrainersListForNewColumn()
        {
            return webClient.Get("api/columnData/getTrainersListForNewColumn", "");
        }

        [HttpPost]
        public async Task<string> CreateNewColumn([FromBody]NewColumnData column)
        {
            string response = String.Empty;

            await Task.Run(() => 
            {
                response = webClient.Send<NewColumnData>(HttpMethod.Post, "api/columnData/createnewColumn", column);
            });

            return response;
        }

        [HttpGet]
        public string GetDriversCount()
        {
            return webClient.Get("api/columnData/getCount", "?entity=drivers");
        }
    }
}
