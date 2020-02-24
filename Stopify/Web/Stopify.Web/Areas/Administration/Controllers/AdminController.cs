using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stopify.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    public abstract class AdminController : Controller
    {
    }
}