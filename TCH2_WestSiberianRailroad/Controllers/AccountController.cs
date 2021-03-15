using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailroadPortalClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TCH2_WestSiberianRailroad.Models;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CurrentAppContext db;
        private readonly IHttpContextAccessor contextAccessor;

        public AccountController(CurrentAppContext context, IHttpContextAccessor httpContext)
        {
            db = context;
            contextAccessor = httpContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> SignIn([FromBody] SignInModel model)
        {
            User user = db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null && user.Password == GetHashImage(model.Password, user.Salt))
            {
                await Authenticate(user.Email);
                await RegisterSession(user.Id);
                if (user.ConfirmedEmail == 0)
                {
                    await SendEmail(user);
                    return "/Account/UnconfirmedAccount";
                }

                return "/Content/Admin";
            }

            return null;
        }

        [HttpPost]
        public async Task<string> SaveUserData([FromBody] UserFullName fullName)
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];

            int userId = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId).UserId;
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.FirstName = fullName.FirstName;
                user.LastName = fullName.LastName;
                user.MiddleName = fullName.MiddleName;
                await db.SaveChangesAsync();
                return "/Content/Admin";
            }

            return String.Empty;
        }

        [HttpPost]
        public async Task<string> CreateNewAccount([FromBody] NewAccountDataModel model)
        {
            if (model.Email != null && model.Email != " ")
            {
                var salt = GetSalt();

                User newUser = new User
                {
                    Email = model?.Email,
                    Salt = salt,
                    Password = GetHashImage("12345", salt),
                    FirstName = model?.FirstName,
                    LastName = model?.LastName,
                    MiddleName = model?.MiddleName,
                    PositionId = model?.PositionId,
                    RoleId = model?.RoleId
                };

                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();

                return newUser.Email;
            }
            else return String.Empty;
        }

        [HttpGet]
        public async Task<string> Logout()
        {
            string sessionId = contextAccessor.HttpContext.Request.Cookies["SessionId"];
            SessionModel session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            session.Expired = DateTime.Now;
            await db.SaveChangesAsync();

            await contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            contextAccessor.HttpContext.Response.Cookies.Delete("SessionId");
            return "/Start/Index";
        }

        [AllowAnonymous]
        public IActionResult ConfirmedAccount(string hashForCheck)
        {
            EmailConfirmModel confirmModel = db.EmailConfirmModels.FirstOrDefault(m => m.HashForCheck == hashForCheck);
            if (confirmModel != null)
            { 
                User user = db.Users.FirstOrDefault(u => u.Id == confirmModel.UserId);

                user.ConfirmedEmail = 1;
                db.EmailConfirmModels.Remove(confirmModel);
                db.SaveChangesAsync();

                //.:: Temporary code -> set redirect by roles
                return user == null ?
                RedirectToAction("Index", "Start") : (IActionResult)View(user);
            }
            return RedirectToAction("Index", "Start");
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
            DateTime currentDateTime = DateTime.Now;

            SessionModel session = new SessionModel
            {
                SessionId = Guid.NewGuid().ToString(),
                UserId = userId,
                Created = currentDateTime,
                Expired = currentDateTime.AddMinutes(15)
            };

            await db.Sessions.AddAsync(session);
            await db.SaveChangesAsync();

            contextAccessor.HttpContext.Response.Cookies.Append("SessionId", session.SessionId);
        }

        private byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private async Task SendEmail(User user)
        {
            await Task.Run(() => 
            {
                SmtpClient client = new SmtpClient("smtp.mail.ru");

                client.UseDefaultCredentials = true;
                client.EnableSsl = true;

                var appEmailAccount = db.AppEmailAccounts.FirstOrDefault(e => e.IsActual == 1);
                client.Credentials = new System.Net.NetworkCredential(appEmailAccount.Email, appEmailAccount.Password);

                MailAddress from = new MailAddress("tch2.westsibrailroad@mail.ru", "Локомотивное депо ТЧ-2 Омск, ЗСЖД");
                MailAddress to = new MailAddress(user.Email, user.FirstName);
                MailMessage message = new MailMessage(from, to);

                MailAddress reply = new MailAddress(user.Email);
                message.ReplyToList.Add(reply);

                message.Subject = "Подтверждение почты для регистрации на железнодорожном веб-портале";
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                string guid = Guid.NewGuid().ToString();
                string hashForCheck = GetHashImage(guid, user.Salt);

                db.EmailConfirmModels.AddAsync(new EmailConfirmModel 
                {
                    UserId = user.Id,
                    HashForCheck = hashForCheck
                });
                db.SaveChangesAsync();

                string urlCallback = "https://localhost:44356/account/confirmedAccount?hashForCheck=" + hashForCheck;
                message.Body = $"<p>Здравствуйте, {user.FirstName}. Для завершения регистрации перейдите по <a href=\""
                                                           + urlCallback + "\">этой ссылке</a>";

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                client.Send(message);
            });
        }
    }
}
