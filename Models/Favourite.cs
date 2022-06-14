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
        public Movie movie { get; set; }
        [ForeignKey("UserInfo")]
        public UserInfo user { get; set; }  


    }
}
