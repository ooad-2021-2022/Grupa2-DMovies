using System;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DMovies.Data;
using DMovies.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DMovies.Controllers
{
    public class EditorController : Controller
    {
        private readonly ILogger<EditorController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public EditorController(ILogger<EditorController> logger, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMovie([FromForm] IFormFile file = null)
        {
            byte[] content = null;
            if (file != null)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    using (var memstream = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(memstream);
                        content = memstream.ToArray();
                    }
                }
            }

            var imdbUrl = HttpContext.Request.Form["imdbUrl"].ToString();
            var imdbMovieId = imdbUrl.Replace("https://www.imdb.com/title/", "").Replace("/", "");

            var movieInfo = new MovieInfo();
            movieInfo.imdbMovieId = imdbMovieId;
            _dbContext.MovieInfos.Add(movieInfo);

            var movie = new Movie();
            movie.data = content;
            movie.contentType = file.ContentType;

            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();


            return RedirectToAction("AddMovie");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private string GetImdbId(string imdbUrl)
        {
            return imdbUrl.Replace("https://", "");
        }
    }
}