using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TCH2_WestSiberianRailroad.Modules.Implementation;
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor contextAccessor;
        private IAccountActions account;
        private TCH2_WebClient webClient;

        public AccountController(IHttpContextAccessor httpContext, IAccountActions _account)
        {
            contextAccessor = httpContext;
            account = _account;
            webClient = new TCH2_WebClient();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> SignIn([FromBody] SignInModel model)
        {
            var response = webClient.Send<SignInModel>(HttpMethod.Post, "api/account/signIn", model);
            User user = JsonConvert.DeserializeObject<User>(response);

            if (user != null && user.Password == GetHashImage(model.Password, user.Salt))
            {
                await Authenticate(user.Email);
                await RegisterSession(user.Id);

                return webClient.Send<User>(HttpMethod.Post, "api/account/confirmEmails", user);
            }

            return null;
        }

        [HttpPost]
        public async Task<string> SaveUserData([FromBody] UserFullName fullName)
        {
            string path = String.Empty;

            await Task.Run(() =>
            {
                User user = account.GetCurrentUser();
                UserFullNameWithId fullNameWithId = new UserFullNameWithId
                {
                    FirstName = fullName.FirstName,
                    LastName = fullName.LastName,
                    MiddleName = fullName.MiddleName,
                    UserId = user.Id
                };

                path = webClient.Send<UserFullNameWithId>(HttpMethod.Post, "api/account/saveAdminFullName", fullNameWithId);
            });

            return path;
        }

        [HttpPost]
        public async Task<string> CreateNewAccount([FromBody] NewAccountDataModel model)
        {
            string email = String.Empty;

            await Task.Run(() =>
            {
                email = webClient.Send<NewAccountDataModel>(HttpMethod.Post, "api/account/createNewAccount", model);
            });

            return email;
        }

        [HttpGet]
        public async Task<string> Logout()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            string path = webClient.Get("api/account/logout", "?sessionId=" + sessionId + "&success=" + true);

            await contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            contextAccessor.HttpContext.Response.Cookies.Delete("SessionId");
            return path;
        }

        [HttpGet]
        public string CheckEmailStatus()
        {
            User user = account.GetCurrentUser();
            if (user.ConfirmedEmail == 1)
            {
                return account.GetUrlUserAccount(user.PositionId);
            }

            return String.Empty;
        }

        [AllowAnonymous]
        public IActionResult ConfirmedAccount(string hashForCheck)
        {
            var response = webClient.Get("api/account/confirmedAccount", "?hashForCheck=" + hashForCheck + "&value=" + 1);
            User user = JsonConvert.DeserializeObject<User>(response);

            if (user != null)
            {
                string pageName = account.GetViewUserAccount(user.PositionId);
            }

            return View(user);
        }

        public IActionResult UnconfirmedAccount()
        {
            return View();
        }

        private string GetHashImage(string pswrd, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pswrd,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private async Task RegisterSession(int userId)
        {
            await Task.Run(() =>
            {
                string jsonSession = webClient.Get("api/account/registerSession", "?userId=" + userId);
                var session = JsonConvert.DeserializeObject<SessionModel>(jsonSession);

                contextAccessor.HttpContext.Response.Cookies.Append("SessionId", session.SessionId);
            });
        }
    }
}
