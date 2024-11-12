using Microsoft.AspNetCore.Mvc;
using OnlineBankingApp.Data;
using OnlineBankingApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineBankingApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts
                                         .Include(a => a.User)
                                         .Include(a => a.Bank)
                                         .ToListAsync();
            return View(accounts);
        }

        // GET: Accounts/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var account = await _context.Accounts
                                        .Include(a => a.User)
                                        .Include(a => a.Bank)
                                        .FirstOrDefaultAsync(a => a.AccountId == id.ToString());

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _context.Accounts.FindAsync(id.ToString());
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account account)
        {
            if (id.ToString() != account.AccountId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(account).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context.Accounts
                                        .FirstOrDefaultAsync(a => a.AccountId == id.ToString());

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id.ToString());
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id.ToString());
        }
    }
}
