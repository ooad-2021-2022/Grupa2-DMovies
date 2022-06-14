using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DMovies.Models
{
    public class JSON
    {
        public string imgPath = "https://image.tmdb.org/t/p/w185";
        public class Movie1
        {
            [JsonInclude]
            public string poster_path { get; set; }
            [JsonInclude]
            public string overview { get; set; }
            [JsonInclude]
            public string release_date { get; set; }
            [JsonInclude]
            public int id { get; set; }
            [JsonInclude]
            public string title     { get; set; }
        }
        public class Search
        {
            [JsonInclude]
            public int page { get; set; }
            [JsonInclude]
            public List<Movie1> results { get; set; }
        }
        public class MoivesD
        {

            [JsonInclude]
            public List<genre> genres { get; set; }
            [JsonInclude]
            public int budget { get; set; }

            [JsonInclude]
            public int runtime { get; set; }
        }
        public class genre
        {
            public string name { get; set; }   
        }
     }
}
