using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;

namespace MovieApp.ViewComponents
{
    public class PopularReviews : ViewComponent
    {
        private readonly IReviewRepository _reviewRepository;

        public PopularReviews(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reviews = await _reviewRepository.Reviews.Include(r => r.Movie).Include(r => r.Likes).Include(r => r.User).OrderByDescending(r => r.Likes.Count).Take(4).ToListAsync();
            return View(reviews);
        }
    }
}