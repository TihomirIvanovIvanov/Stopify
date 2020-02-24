using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Stopify.Web.Areas.Administration.Controllers
{
    public class ProductController : AdminController
    {
        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create()
        {
            return null;
        }
    }
}