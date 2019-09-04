using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDoData.Models
{
    public class Movie
    {
        public Movie()
        {
            Reviews = new List<Review>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plot { get; set; }
        public string Director { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Genre Genre { get; set; }
        public string ReleaseDate { get; set; }

        public ICollection<Review> Reviews { get; set; }
}

    public enum Genre
    {
        Action,
        Adventure
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
