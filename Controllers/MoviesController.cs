using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private HttpClient httpClient = new HttpClient();
        private readonly ApplicationDbContext _context;
        private string apikey = "574fa1986673102f483efa843989bba6";
        private string path = "https://image.tmdb.org/t/p/w185/";

        public object JasonSerialize { get; private set; }

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {

            List<Movie> movies = new List<Movie>();//await _context.Movies.ToListAsync();

            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };

                string responseBody = await httpClient.GetStringAsync("https://api.themoviedb.org/3/movie/popular?api_key=574fa1986673102f483efa843989bba6&language=en-US&page=1");
                /*  string responseBody1 = await httpClient.GetStringAsync(
                      "https://api.themoviedb.org/3/movie/%7Bmovie_id%7D?api_key=574fa1986673102f483efa843989bba6&language=en-U");
                */
                var mov = JsonSerializer.Deserialize<Search>(responseBody, options)!.results;
                if(mov!=null)
                for (int i = 0; i < mov.Count; i++)
                {
                    Movie mk = new Movie();
                    mk.rating = mov[i].id;
                    mk.streamLink = "https://image.tmdb.org/t/p/w185/" + mov[i].poster_path;
                    mk.name = mov[i].title;
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


        public async Task<IActionResult> Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchResults([Bind("name")] Movie  list)
        {
            List<Movie> movies = new List<Movie>();
          
               string t= list.name;
            Search mov=null;
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };
                var byt=System.Text.Encoding.ASCII.GetBytes(t);
                var quer = System.Text.Encoding.UTF8.GetString(byt);
                string responseBody = await httpClient.GetStringAsync("https://api.themoviedb.org/3/search/movie?api_key=574fa1986673102f483efa843989bba6&language=en-US&page=1&include_adult=false&query="+quer);
                
                 mov = JsonSerializer.Deserialize<Search>(responseBody, options)!;
                for (int i = 0; i < mov.results.Count; i++)
                {
                    Movie mk = new Movie();
                    mk.rating = mov.results[i].id;
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
            Movie mk5 = new Movie();
            mk5.name = t;
            mk5.streamLink = t;
            mk5.rating = mov.results.Count;
            movies.Add(mk5);
            return View(movies.AsEnumerable<Movie>());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
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

            var movie = await _context.Movies.FindAsync(id);
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

            var movie = await _context.Movies
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
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("Download")]
        public async Task<IActionResult> DownloadMovie([FromRoute] int id)
        {
            Movie movie;
            try
            {
                movie = await _context.Movies.Where(m => m.Id == id).FirstAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }

            Console.WriteLine(movie.contentType);
            
            return File(movie.data, movie.contentType);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}