using Microsoft.AspNetCore.Mvc;

namespace Stopify.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
