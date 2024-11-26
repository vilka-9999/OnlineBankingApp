using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineBankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineBankingAppContext _context;
        private int? _userId; // Global variable to store the userId

        public HomeController(ILogger<HomeController> logger, OnlineBankingAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Index Action
        [Authorize]
        public IActionResult Index()
        {
            var userEmail = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Error", "Home");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Store the userId in ViewBag
            ViewBag.AUserId = user.UserId;

            var accounts = _context.Accounts
                                   .Where(a => a.UserId == user.UserId)
                                   .Include(a => a.Bank)
                                   .ToList();

            ViewBag.User = user;
            ViewBag.Accounts = accounts;
            return View();
        }

        // GET: Create Account
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            ViewBag.Banks = _context.Banks.ToList(); // Populate banks for dropdown
            return View(new Account()); // Return an empty Account model to the view
        }

        // POST: Create Account
        [HttpPost]
        [Authorize]
        public IActionResult Create(Account account)
        {
            var userEmail = HttpContext.User.Identity?.Name;

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            var userId = user.UserId;

            if (ModelState.IsValid)
            {
                // Retrieve the userId from ViewBag (set in Index action)
                
                if (userId.HasValue)
                {
                    // Assign the userId to the account
                    account.UserId = userId.Value;

                    // Check for valid bank and account balance
                    if (account.BankId == 0 || account.AccountBalance <= 0)
                    {
                        ModelState.AddModelError("", "Account must have a valid bank and positive balance.");
                        ViewBag.Action = "Create";
                        ViewBag.Banks = _context.Banks.ToList();
                        return View(account); // Return to the view with errors
                    }

                    // Add the new account to the database
                    _context.Accounts.Add(account);
                    _context.SaveChanges(); // Save changes to the database

                    return RedirectToAction("Index"); // Redirect to the Index view after creation
                }
                else
                {
                    ModelState.AddModelError("", "Unable to associate this account with the user.");
                    ViewBag.Action = "Create";
                    ViewBag.Banks = _context.Banks.ToList();
                    return View(account); // Return to the view if userId is not found
                }
            }

            // If validation fails, reload the banks list
            ViewBag.Action = "Create";
            ViewBag.Banks = _context.Banks.ToList();
            return View(account); // Return the account model to the view if validation fails
        }
    }
}
