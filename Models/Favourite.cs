using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    [Keyless]
    public class Favourite 
    {
        public Favourite()
        {
        }
        
        [ForeignKey("Movie")]
        public int movieId { get; set; }
        public Movie movie { get; set; }
        [ForeignKey("UserInfo")]
        public int userId { get; set; }
        public UserInfo user { get; set; }  


    }
}
