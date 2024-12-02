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
                return RedirectToAction("Index", "UserRegistrationLogin");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Index", "UserRegistrationLogin");
            }

            // Store the userId in ViewBag
            ViewBag.AUserId = user.UserId;

            var accounts = _context.Accounts
                                   .Where(a => a.UserId == user.UserId)
                                   .Where(a => !a.IsDeleted)
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
                    if (account.BankId == 0 || account.AccountBalance < 0)
                    {
                        ModelState.AddModelError("", "Account must have a valid bank and positive balance.");
                        ViewBag.Action = "Create";
                        ViewBag.Banks = _context.Banks.ToList();
                        return View(account); // Return to the view with errors
                    }


                    var checkAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == account.AccountNumber);
                    if (checkAccount!= null && !checkAccount.IsDeleted)
                    {
                        ModelState.AddModelError("AccountNumber", "Account already exists");
                        ViewBag.Action = "Create";
                        ViewBag.Banks = _context.Banks.ToList();
                        return View(account); // Return to the view with errors
                    }

                    // update isDeleted field or create a new account
                    if (checkAccount == null)
                    {
                        _context.Accounts.Add(account);
                    }
                    else
                    {
                        checkAccount.IsDeleted = false;
                        _context.Update(checkAccount);
                    }
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
            ModelState.AddModelError("", "Error");
            ViewBag.Banks = _context.Banks.ToList();
            return View(account); // Return the account model to the view if validation fails
        }


        [HttpGet]
        [Authorize]
        public IActionResult EditBalance(int id)
        {
            ViewBag.Action = "Edit Balance";
            var account = _context.Accounts.Find(id);
            return View(account);
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditBalance(Account account)
        {

            var userEmail = HttpContext.User.Identity?.Name;

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            var userId = user.UserId;


            if (ModelState.IsValid)
            {
                // check if the account is edited by its user
                if (userId == account.UserId)
                {

                    // Check for valid bank and account balance
                    if (account.AccountBalance < 0)
                    {
                        ModelState.AddModelError("", "Account must have a positive balance.");
                        ViewBag.Action = "Create";
                        
                        return View(account); // Return to the view with errors
                    }

                    // Update the new account to the database
                    _context.Update(account);
                    _context.SaveChanges(); // Save changes to the database

                    return RedirectToAction("Index"); // Redirect to the Index view after creation
                }
            }

            return View(account); // Return the account model to the view if validation fails
        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            ViewBag.Action = "Delete Account";
            var account = _context.Accounts.Find(id);
            return View(account);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(Account account)
        {
            account.IsDeleted = true;
            _context.Update(account);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }


}
