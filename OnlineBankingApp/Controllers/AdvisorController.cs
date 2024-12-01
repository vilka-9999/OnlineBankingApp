using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApp.Models;

namespace OnlineBankingApp.Controllers
{
    [Authorize]
    public class AdvisorController : Controller
    {

        private readonly ILogger<AdvisorController> _logger;
        private readonly OnlineBankingAppContext _context;

        public AdvisorController(ILogger<AdvisorController> logger, OnlineBankingAppContext context)
        {
            _logger = logger;
            _context = context;
        }

    
        public IActionResult Index()
        {
            var userEmail = HttpContext.User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            var advisor = _context.Advisors.Find(user.AdvisorId);
            return View(advisor);
        }
    }
}
