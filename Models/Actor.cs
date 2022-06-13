using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    [Keyless]
    public class Actor
    {
        [ForeignKey("MovieInfo")]
           public int IDMovieInfo { get; set; } 
        public   MovieInfo MovieInfo { get; set; }  
        public string Name { get; set; }    
    }
}
