using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MovieApp.Entities;

namespace MovieApp.Models
{
    public class MovieUpdateViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Please enter a movie name")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a movie description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter a movie release date")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Please enter a movie director")]
        public long DirectorId { get; set; }
        public long[] GenreIds { get; set; }
        public long[] ActorIds { get; set; }
        public string? Image { get; set; }
    }
}