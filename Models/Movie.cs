using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    public class Movie
    {
        public Movie()
        {

        }
        [Key]
        public int Id { get; set; }
        public string name { get; set; }    
        public double rating { get; set; }
        [ForeignKey("DownloadLinks")]
        public int downloadLinkId { get; set; }
        public DownloadLinks links { get; set; }
        public string streamLink { get; set; }
        [ForeignKey("MovieInfo")]
        public int movieInfoId { get; set; }
        public MovieInfo movieInfo { get; set; }    

    }
}
