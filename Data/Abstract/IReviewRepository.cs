using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IReviewRepository
    {
        IQueryable<Review> Reviews { get; }
        
        void SaveReview(Review entity);
        
    }
}