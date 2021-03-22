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
using TCH2_WestSiberianRailroad.Modules.Interfaces;

namespace TCH2_WestSiberianRailroad.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CurrentAppContext db;
        private readonly IHttpContextAccessor contextAccessor;
        private IAccountActions account;

        public AccountController(CurrentAppContext context, IHttpContextAccessor httpContext, IAccountActions _account)
        {
            db = context;
            contextAccessor = httpContext;
            account = _account;
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
                    var confirmEmails = db.EmailConfirmModels.Where(m => m.UserId == user.Id).ToArray();
                    if (confirmEmails != null)
                    {
                        db.EmailConfirmModels.RemoveRange(confirmEmails);
                    }

                    await SendEmail(user);
                    return "/Account/UnconfirmedAccount";
                }

                return account.GetUrlUserAccount(user.PositionId);
            }

            return null;
        }

        [HttpPost]
        public async Task<string> SaveUserData([FromBody] UserFullName fullName)
        {
            User user = account.GetCurrentUser();

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
                    PositionId = model.PositionId,
                    RoleId = model?.RoleId,
                    IsActual = 1
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
            var model = db.EmailConfirmModels.Where(m => m.HashForCheck == hashForCheck).ToArray();
            User user = db.Users.FirstOrDefault(u => u.Id == model[0].UserId);

            if (user != null)
            {
                user.ConfirmedEmail = 1;
                db.EmailConfirmModels.Remove(model[0]);
                db.SaveChanges();

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

                string hashForCheck = Guid.NewGuid().ToString();

                db.EmailConfirmModels.AddAsync(new EmailConfirmModel 
                {
                    UserId = user.Id,
                    HashForCheck = hashForCheck
                });
                db.SaveChangesAsync();

                string urlCallback = "https://localhost:44356/account/confirmedAccount?hashForCheck="+hashForCheck;
                message.Body = $"<p>Здравствуйте, {user.FirstName}. Для завершения регистрации перейдите по <a href=\""
                                                           + urlCallback + "\">этой ссылке</a>";

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                client.Send(message);
            });
        }
    }
}
