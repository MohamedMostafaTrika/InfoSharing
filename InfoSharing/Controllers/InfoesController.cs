using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfoSharing.Data;
using InfoSharing.Models;
using Microsoft.AspNetCore.Authorization;

namespace InfoSharing.Controllers
{
    public class InfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Infoes
        public async Task<IActionResult> Index()
        {
              return _context.Info != null ? 
                          View(await _context.Info.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Info'  is null.");
        }

        // GET: Infoes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Info != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Info'  is null.");
        }
        // Post: Infoes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return _context.Info != null ?
                        View("Index", await _context.Info.Where(j =>j.InfoQuestion.Contains(SearchPhrase)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Info'  is null.");
        }
        // GET: Infoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Info == null)
            {
                return NotFound();
            }

            var info = await _context.Info
                .FirstOrDefaultAsync(m => m.id == id);
            if (info == null)
            {
                return NotFound();
            }

            return View(info);
        }

        // GET: Infoes/Create
        [Authorize]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Infoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,InfoQuestion,InfoAnswer")] Info info)
        {
            if (ModelState.IsValid)
            {
                _context.Add(info);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(info);
        }

        // GET: Infoes/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Info == null)
            {
                return NotFound();
            }

            var info = await _context.Info.FindAsync(id);
            if (info == null)
            {
                return NotFound();
            }
            return View(info);
        }

        // POST: Infoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,InfoQuestion,InfoAnswer")] Info info)
        {
            if (id != info.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(info);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfoExists(info.id))
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
            return View(info);
        }

        // GET: Infoes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Info == null)
            {
                return NotFound();
            }

            var info = await _context.Info
                .FirstOrDefaultAsync(m => m.id == id);
            if (info == null)
            {
                return NotFound();
            }

            return View(info);
        }

        // POST: Infoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Info == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Info'  is null.");
            }
            var info = await _context.Info.FindAsync(id);
            if (info != null)
            {
                _context.Info.Remove(info);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InfoExists(int id)
        {
          return (_context.Info?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
