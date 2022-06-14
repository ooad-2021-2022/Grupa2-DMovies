using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{

    public class MovieInfo
    {
        public MovieInfo()
        {
        }
        [Key]
        public int Id { get; set; }
        public string synopsis { get; set; }
        public string director { get; set; }
                                                    
    }
}
