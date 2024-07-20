using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MovieDbContext _context;

        public LikeRepository(MovieDbContext context)
        {
            _context = context;
        }
        public IQueryable<Like> Likes => _context.Likes;

        public void DeleteLike(Like like)
        {
            _context.Likes.Remove(like);
            _context.SaveChanges();
        }

        public void SaveLike(Like like)
        {
            _context.Likes.Add(like);
            _context.SaveChanges();
        }
    }

}