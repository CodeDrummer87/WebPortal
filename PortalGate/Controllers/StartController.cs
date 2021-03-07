using Microsoft.AspNetCore.Mvc;

namespace PortalGate.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
