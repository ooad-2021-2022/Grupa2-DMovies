using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DMovies.Data;
using DMovies.Models;

namespace DMovies.Controllers
{
    public class CommentRatingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentRatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CommentRatings
        public async Task<IActionResult> Index()
        {
            return View(await _context.CommentRating.ToListAsync());
        }

        // GET: CommentRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentRating = await _context.CommentRating
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentRating == null)
            {
                return NotFound();
            }

            return View(commentRating);
        }

        // GET: CommentRatings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CommentRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,reports,rating,comment")] CommentRating commentRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentRating);
        }

        // GET: CommentRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentRating = await _context.CommentRating.FindAsync(id);
            if (commentRating == null)
            {
                return NotFound();
            }
            return View(commentRating);
        }

        // POST: CommentRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,reports,rating,comment")] CommentRating commentRating)
        {
            if (id != commentRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentRatingExists(commentRating.Id))
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
            return View(commentRating);
        }

        // GET: CommentRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentRating = await _context.CommentRating
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentRating == null)
            {
                return NotFound();
            }

            return View(commentRating);
        }

        // POST: CommentRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentRating = await _context.CommentRating.FindAsync(id);
            _context.CommentRating.Remove(commentRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentRatingExists(int id)
        {
            return _context.CommentRating.Any(e => e.Id == id);
        }
    }
}
