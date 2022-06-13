using System.ComponentModel.DataAnnotations;

namespace DMovies.Models
{
    public class DownloadLinks
    {
        public DownloadLinks()
        {

        }
        [Key]
        public int Id { get; set; }     
        public string Url { get; set; }
        public int resolution { get; set; }
    }
}
