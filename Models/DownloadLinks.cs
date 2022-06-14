using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    [Keyless]
    public class DownloadLinks
    {
        public DownloadLinks()
        {

        }
        [ForeignKey("Movie")]
        public Movie movie { get; set; }    
        public string Url { get; set; }
        public int resolution { get; set; }
    }
}
