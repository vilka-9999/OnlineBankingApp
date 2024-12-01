using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApp.Models;

namespace OnlineBankingApp.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private readonly OnlineBankingAppContext _context;

        public TransferController(OnlineBankingAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userEmail = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "UserRegistrationLogin");
            }

            // Retrieve the logged-in user's information based on email
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Index", "UserRegistrationLogin");
            }

            // Pass user ID and account data to the view
            ViewBag.UserId = user.UserId;
            var accounts = _context.Accounts
                                   .Where(a => a.UserId == user.UserId)
                                   .Include(a => a.Bank)
                                   .ToList();

            ViewBag.User = user;
            ViewBag.Accounts = accounts;
            return View();
        }

        [HttpPost]
        public IActionResult Index(int senderAccountId, long receiverAccountNumber, decimal transferAmount)
        {
            // Fetch the sender's account
            var senderAccount = _context.Accounts.FirstOrDefault(a => a.AccountId == senderAccountId);
            if (senderAccount == null)
            {
                ModelState.AddModelError("", "Sender account not found.");
                return View();
            }

            // Ensure the sender has enough balance
            if (senderAccount.AccountBalance < transferAmount)
            {
                ModelState.AddModelError("", "Insufficient balance in the sender's account.");
                return View();
            }

            // Fetch the receiver's account by account number
            var receiverAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == receiverAccountNumber);
            if (receiverAccount == null)
            {
                ModelState.AddModelError("", "Receiver account not found.");
                return View();
            }

            // Perform the transfer
            senderAccount.AccountBalance -= transferAmount;
            receiverAccount.AccountBalance += transferAmount;

            // Save changes to the database
            _context.SaveChanges();

            // Notify success and reload the view
            ViewBag.SuccessMessage = "Transfer completed successfully!";
            return RedirectToAction("Index");
        }
    }
}
