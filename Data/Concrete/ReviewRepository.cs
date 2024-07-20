using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieDbContext _context;

        public ReviewRepository(MovieDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<Review> Reviews => _context.Reviews;

        public void SaveReview(Review entity)
        {
            _context.Reviews.Add(entity);
            _context.SaveChanges();
        }
    }
}