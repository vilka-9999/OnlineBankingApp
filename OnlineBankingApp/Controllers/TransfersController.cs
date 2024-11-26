using Microsoft.AspNetCore.Mvc;

namespace OnlineBankingApp.Controllers
{
    public class TransfersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
