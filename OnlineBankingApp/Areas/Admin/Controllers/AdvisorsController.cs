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
    public class AdvisorsController : Controller
    {
        private readonly OnlineBankingAppContext _context;

        public AdvisorsController(OnlineBankingAppContext context)
        {
            _context = context;
        }

        // GET: Admin/Advisors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Advisors.ToListAsync());
        }

        // GET: Admin/Advisors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors
                .FirstOrDefaultAsync(m => m.AdvisorId == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // GET: Admin/Advisors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Advisors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdvisorId,AdvisorName,AdvisorPhone,AdvisorEmail,ClientsNumber")] Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advisor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advisor);
        }

        // GET: Admin/Advisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null)
            {
                return NotFound();
            }
            return View(advisor);
        }

        // POST: Admin/Advisors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("AdvisorId,AdvisorName,AdvisorPhone,AdvisorEmail,ClientsNumber")] Advisor advisor)
        {
            if (id != advisor.AdvisorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorExists(advisor.AdvisorId))
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
            return View(advisor);
        }

        // GET: Admin/Advisors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors
                .FirstOrDefaultAsync(m => m.AdvisorId == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // POST: Admin/Advisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor != null)
            {
                _context.Advisors.Remove(advisor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvisorExists(int? id)
        {
            return _context.Advisors.Any(e => e.AdvisorId == id);
        }
    }
}
