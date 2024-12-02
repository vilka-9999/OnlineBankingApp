using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OnlineBankingApp.Models;
using System.Security.Cryptography.Xml;

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
            
            var accounts = _context.Accounts
                                   .Where(a => a.UserId == user.UserId)
                                   .Where(a => !a.IsDeleted)
                                   .Include(a => a.Bank)
                                   .ToList();

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
                TempData["TransferUnsuccess"] = "Sender account not found.";
                return RedirectToAction("Index");
            }

            // Ensure the sender has enough balance
            if (senderAccount.AccountBalance < transferAmount)
            {
                TempData["TransferUnsuccess"] = "Insufficient balance in the sender's account.";
                return RedirectToAction("Index");
            }

            // Fetch the receiver's account by account number
            var receiverAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == receiverAccountNumber);
            if (receiverAccount == null || receiverAccount.IsDeleted)
            {
                TempData["TransferUnsuccess"] = "Receiver account not found";
                return RedirectToAction("Index");
            }

            // Perform the transfer
            senderAccount.AccountBalance -= transferAmount;
            receiverAccount.AccountBalance += transferAmount;

            var transfer = new Transfer
            {
                SenderAccountId = senderAccountId,
                ReceiverAccountId = receiverAccount.AccountId,
                TransferAmount = transferAmount,
            };

            // Save changes to the database
            _context.Add(transfer);
            _context.SaveChanges();

            // Notify success and reload the view
            TempData["SuccessMessage"] = "Transfer completed successfully!"; // need TempData because of different request
            return RedirectToAction("Index", "Home");
        }


        public IActionResult TransferHistory(int id)
        {
            var transfers = _context.Transfers
                .Include(t => t.ReceiverAccount)
                .Include(t => t.SenderAccount)
                .Where(t => t.SenderAccountId == id || t.ReceiverAccountId == id)
                .OrderByDescending(t => t.TransferDate)
                .ToList();
            var account = _context.Accounts.Find(id);
            ViewBag.AccountNumber = account.AccountNumber;
            return View(transfers);
        }
    }
}
