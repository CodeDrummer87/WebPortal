using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;
using System;
using System.Net.Http;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private IAccountActions account;
        private ITCH2_WebClient webClient;

        public ContentController(IAccountActions _account, ITCH2_WebClient _webClient)
        {
            account = _account;
            webClient = _webClient;
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
            return webClient.Get("api/content/getEntity", "?page=" + page + "&isActual=" + isActual + "&entityType=employees");
        }

        [HttpGet]
        public string GetPositions(int page, byte isActual)
        {
            return webClient.Get("api/content/getEntity", "?page=" + page + "&isActual=" + isActual + "&entityType=positions");
        }

        [HttpGet]
        public string GetRoles(int page, byte isActual)
        {
            return webClient.Get("api/content/getEntity", "?page=" + page + "&isActual=" + isActual + "&entityType=roles");
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
            string response = webClient.Get("api/content/getCount", "?isActual=" + isActual + "&entity=users");
            return Convert.ToInt32(response);
        }

        [HttpGet]
        public int GetPositionCount(byte isActual)
        {
            string response = webClient.Get("api/content/getCount", "?isActual=" + isActual + "&entity=positions");
            return Convert.ToInt32(response);
        }

        [HttpGet]
        public int GetRoleCount(byte isActual)
        {
            string response = webClient.Get("api/content/getCount", "?isActual=" + isActual + "&entity=roles");
            return Convert.ToInt32(response);
        }

        [HttpGet]
        public int GetEmailCount()
        {
            byte isActual = 1;
            string response = webClient.Get("api/content/getCount", "?isActual=" + isActual + "&entity=emails");
            return Convert.ToInt32(response);
        }

        [HttpGet]
        public string GetAppEmailAccount(int page)
        {
            return webClient.Get("api/content/getAppEmailAccount", "?page=" + page);
        }

        [HttpGet]
        public string GetCurrentUserById(int userId)
        {
            return webClient.Get("api/content/getDataById", "?id=" + userId + "&dataType=user");
        }

        [HttpPut]
        public string UpdateEmployeeData(int userId, string email, string firstName, string lastName, string middleName, int positionId, int roleId)
        {
            return webClient.Put("api/content/updateEmployeeData", "?userId=" + userId + "&email=" + email + "&firstName=" + firstName
                + "&lastName=" + lastName + "&middleName=" + middleName + "&positionId=" + positionId + "&roleId=" + roleId);
        }

        [HttpDelete]
        public string RemoveEmployee(int userId)
        {
            return webClient.Delete("api/content/removeDataById", "?id=" + userId + "&dataType=user");
        }

        [HttpPut]
        public string RecoverEmployeeFromArchive(int userId)
        {
            return webClient.Put("api/content/recoverDataById", "?id=" + userId + "&dataType=user");
        }

        [HttpPost]
        public string SaveNewPosition([FromBody] NewPositionData data)
        {
            return webClient.Send<NewPositionData>(HttpMethod.Post, "api/content/saveNewPosition", data);
        }

        [HttpGet]
        public string GetCurrentPositionById(int positionId)
        {
            return webClient.Get("api/content/getDataById", "?id=" + positionId + "&dataType=position");
        }

        [HttpPut]
        public string UpdatePositionData(int positionId, string positionName, string abbreviation)
        {
            return webClient.Put("api/content/updatePositionData", "?positionId=" + positionId + "&positionName=" + positionName
                + "&abbreviation=" + abbreviation);
        }

        [HttpDelete]
        public string RemovePosition(int positionId)
        {
            return webClient.Delete("api/content/removeDataById", "?id=" + positionId + "&dataType=position");
        }

        [HttpPut]
        public string RecoverPositionFromArchive(int positionId)
        {
            return webClient.Put("api/content/recoverDataById", "?id=" + positionId + "&dataType=position");
        }
    }
}
