using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MovieApp.Entities;

namespace MovieApp.Models
{
    public class MovieCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 1000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile? ImageFile { get; set; }

        public List<Genre> Genres { get; set; } = new List<Genre>();

        [Required(ErrorMessage = "Release Date is required")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Director is required")]
        public long DirectorId { get; set; }

        [Required(ErrorMessage = "Actors are required")]
        public List<long> ActorIds { get; set; } = new List<long>();
    }
}
