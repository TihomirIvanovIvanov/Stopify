using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Web.ViewModels.Receipt.Details;
using Stopify.Web.ViewModels.Receipt.Profile;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> Profile()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var receiptFromDb = await this.receiptService
                .GetAllByRecipientId(userId).ToListAsync();

            var receiptsForCurrentUser = receiptFromDb.Select(receipt =>
                receipt.To<ReceiptProfileViewModel>()).ToList();

            return this.View(receiptsForCurrentUser);
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