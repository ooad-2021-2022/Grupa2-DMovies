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
        public Movie movie { get; set; }
        [ForeignKey("UserInfo")]
        public UserInfo userInfo{ get; set; }
                    

    }
}
