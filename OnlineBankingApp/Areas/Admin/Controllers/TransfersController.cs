using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApp.Models;

namespace OnlineBankingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TransfersController : Controller
    {
        private readonly OnlineBankingAppContext _context;

        public TransfersController(OnlineBankingAppContext context)
        {
            _context = context;
        }

        // GET: Admin/Transfers
        public async Task<IActionResult> Index()
        {
            var onlineBankingAppContext = _context.Transfers.Include(t => t.ReceiverAccount).Include(t => t.SenderAccount);
            return View(await onlineBankingAppContext.ToListAsync());
        }

        // GET: Admin/Transfers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.ReceiverAccount)
                .Include(t => t.SenderAccount)
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // GET: Admin/Transfers/Create
        public IActionResult Create()
        {
            ViewData["ReceiverAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["SenderAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Admin/Transfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferId,TransferAmount,TransferDate,SenderAccountId,ReceiverAccountId")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiverAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.ReceiverAccountId);
            ViewData["SenderAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.SenderAccountId);
            return View(transfer);
        }

        // GET: Admin/Transfers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["ReceiverAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.ReceiverAccountId);
            ViewData["SenderAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.SenderAccountId);
            return View(transfer);
        }

        // POST: Admin/Transfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("TransferId,TransferAmount,TransferDate,SenderAccountId,ReceiverAccountId")] Transfer transfer)
        {
            if (id != transfer.TransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.TransferId))
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
            ViewData["ReceiverAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.ReceiverAccountId);
            ViewData["SenderAccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transfer.SenderAccountId);
            return View(transfer);
        }

        // GET: Admin/Transfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfers
                .Include(t => t.ReceiverAccount)
                .Include(t => t.SenderAccount)
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Admin/Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer != null)
            {
                _context.Transfers.Remove(transfer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int? id)
        {
            return _context.Transfers.Any(e => e.TransferId == id);
        }
    }
}
