using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Receipt.Details;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Web.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IReceiptService receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var receiptServiceModel = await this.receiptService
                .GetAll().FirstOrDefaultAsync(receipt => receipt.Id == id);

            var receiptDetailsViewModel = receiptServiceModel.To<ReceiptDetailsViewModel>();

            return this.View(receiptDetailsViewModel);
        }
    }
}