using System.ComponentModel.DataAnnotations.Schema;

namespace DMovies.Models
{
    public class CommentRating
    {
        public CommentRating()
        {

        }
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int reports { get; set; }
        public double rating { get; set; }
        public string comment { get; set; }
        [ForeignKey("Movie")]
        public int movieId { get; set; }
        public Movie movie { get; set; }
        [ForeignKey("UserInfo")]
        public int userId { get; set; }
        public UserInfo userInfo{ get; set; }
                    

    }
}
