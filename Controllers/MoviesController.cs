﻿using System;
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
            List<Movie> movies = await _context.Movies.ToListAsync();
            List<MovieInfo> movieInfos = new List<MovieInfo>();
            IEnumerable<MovieData> movieDatas = null;

            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                movieInfos.Add(await _context.MovieInfos.Where(mi => mi.Id == movie.movieInfoId).FirstAsync());
            }

            try
            {
                movieDatas = await Task.WhenAll(movies.Select(async (m, i) =>
                {
                    var movieInfo = movieInfos[i];
                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true
                    };
                    string responseBody =
                        await httpClient.GetStringAsync("https://imdb-api.com/api/title/k_le4b5uud/" +
                                                        movieInfo.imdbMovieId);
                    var mov = JsonSerializer.Deserialize<IMDBMovie>(responseBody, options)!;
                    Console.WriteLine(responseBody);
                    Console.WriteLine("\n\n");
                    return new MovieData
                    {
                        title = movies[i].name ?? mov.title,
                        imageUrl = mov.image,
                        id = m.Id
                    };
                }));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }

            return View(movieDatas);
        }

        public async Task<IActionResult> Search()
        {
            return View();
        }
        public async Task<IActionResult> Favourite()
        {
            List<Movie> movies = new List<Movie>();

           
            Search mov = null;
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };
               
                string responseBody = await httpClient.GetStringAsync(
                    "https://api.themoviedb.org/3/movie/upcoming?api_key=574fa1986673102f483efa843989bba6&language=en-US&page=1");

                mov = JsonSerializer.Deserialize<Search>(responseBody, options)!;
                for (int i = 0; i < mov.results.Count; i++)
                {
                    Movie mk = new Movie();
                    mk.Id = mov.results[i].id;
                    mk.streamLink = "https://image.tmdb.org/t/p/w185/" + mov.results[i].poster_path;
                    mk.name = mov.results[i].title;
                    if (mov.results[i].title == null)
                        mk.name = " ";
                    if (mov.results[i].poster_path == null)
                        mk.streamLink = " ";
                    movies.Add(mk);
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return View(movies.AsEnumerable<Movie>());
        }

        public async Task<IActionResult> SearchResults([Bind("name")] Movie list)
        {
            List<Movie> movies = new List<Movie>();

            string t = list.name;
            Search mov = null;
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };
                var byt = System.Text.Encoding.ASCII.GetBytes(t);
                var quer = System.Text.Encoding.UTF8.GetString(byt);
                string responseBody = await httpClient.GetStringAsync(
                    "https://api.themoviedb.org/3/search/movie?api_key=574fa1986673102f483efa843989bba6&language=en-US&page=1&include_adult=false&query=" +
                    quer);

                mov = JsonSerializer.Deserialize<Search>(responseBody, options)!;
                for (int i = 0; i < mov.results.Count; i++)
                {
                    Movie mk = new Movie();
                    mk.Id= mov.results[i].id;
                    mk.streamLink = "https://image.tmdb.org/t/p/w185/" + mov.results[i].poster_path;
                    mk.name = mov.results[i].title;
                    if (mov.results[i].title == null)
                        mk.name = " ";
                    if (mov.results[i].poster_path==null)
                        mk.streamLink = " ";
                    movies.Add(mk);
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
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

public class MovieData
{
    public string title { get; set; }
    public string imageUrl { get; set; }
    public int id { get; set; }
}