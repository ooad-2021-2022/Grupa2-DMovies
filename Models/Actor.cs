using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MovieInfo")]
        public int IDMovieInfo { get; set; } 
        public MovieInfo MovieInfo { get; set; }  
        public ICollection<Movie> Movies { get; set; }
        public string Name { get; set; }    
    }
}
