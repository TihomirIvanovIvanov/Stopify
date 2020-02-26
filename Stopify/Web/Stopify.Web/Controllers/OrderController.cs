using Microsoft.AspNetCore.Mvc;

namespace Stopify.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}