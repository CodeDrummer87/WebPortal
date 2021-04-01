using ExternalAPI.DatabaseContext;
using ExternalAPI.Modules.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RailroadPortalClassLibrary;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ExternalAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private CurrentAppContext db;
        private IAccountActions account;

        public AccountController(CurrentAppContext context, IAccountActions _account)
        {
            db = context;
            account = _account;
        }

        [Route("signIn")]
        [HttpPost]
        public string Post([FromBody] SignInModel model)
        {
            User user = db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null && user.Password == GetHashImage(model.Password, user.Salt))
            {
                return JsonConvert.SerializeObject(user);
            }

            return null;
        }

        [Route("confirmEmails")]
        [HttpPost]
        public async Task<string> Post([FromBody] User user)
        {
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

        [Route("getCurrentUser")]
        [HttpGet]
        public string Get(string sessionId)
        {
            int userId = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId).UserId;
            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return JsonConvert.SerializeObject(user);
            }

            return String.Empty;
        }

        [Route("saveAdminFullName")]
        [HttpPost]
        public async Task<string> Post([FromBody] UserFullNameWithId fullName)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == fullName.UserId);

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

        [Route("createNewAccount")]
        [HttpPost]
        public async Task<string> Post([FromBody] NewAccountDataModel model)
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

        [Route("logout")]
        [HttpGet]
        public async Task<string> Get(string sessionId, bool success)
        {
            SessionModel session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            session.Expired = DateTime.Now;
            await db.SaveChangesAsync();

            return "/Start/Index";
        }

        [Route("confirmedAccount")]
        [HttpGet]
        public string Get(string hashForCheck, int value)
        {
            var model = db.EmailConfirmModels.Where(m => m.HashForCheck == hashForCheck).ToArray();
            User user = db.Users.FirstOrDefault(u => u.Id == model[0].UserId);

            if (user != null)
            {
                user.ConfirmedEmail = 1;
                db.EmailConfirmModels.Remove(model[0]);
                db.SaveChanges();

                return JsonConvert.SerializeObject(user);
            }

            return null;
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

        [Route("registerSession")]
        [HttpGet]
        public async Task<string> Get(int userId)
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

            return JsonConvert.SerializeObject(session);
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
