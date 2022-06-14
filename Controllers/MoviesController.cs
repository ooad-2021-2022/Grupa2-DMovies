using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DMovies.Data;
using DMovies.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text.Json;
using static DMovies.Models.JSON;

namespace DMovies.Controllers
{
    public class MoviesController : Controller
    {
        private HttpClient httpClient=new HttpClient();
        private readonly ApplicationDbContext _context  ;
        private string apikey = "574fa1986673102f483efa843989bba6";

        public object JasonSerialize { get; private set; }

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await _context.Movie.ToListAsync();
           
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true,
                    };
                
                    string responseBody = await httpClient.GetStringAsync("https://api.themoviedb.org/3/search/movie?api_key=574fa1986673102f483efa843989bba6&language=en-US&page=1&include_adult=false&query=moj%20moj");
                string responseBody1 = await httpClient.GetStringAsync("https://api.themoviedb.org/3/movie/%7Bmovie_id%7D?api_key=574fa1986673102f483efa843989bba6&language=en-U");

                var mov = JsonSerializer.Deserialize<Search>(responseBody, options)!;
                for (int i = 0; i < mov.results.Count; i++)
                {
                    Movie mk = new Movie();
                    mk.rating=mov.results[i].id;
                    mk.streamLink = mov.results[i].release_date;
                    mk.name = mov.results[i].title;
                    movies.Add(mk);
                }
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
       
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "admin, editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> Create([Bind("Id,name,rating,streamLink")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5

        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,rating,streamLink")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5

        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
