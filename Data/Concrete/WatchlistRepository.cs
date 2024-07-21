using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class WatchlistRepository : IWatchlistRepository
    {

        private MovieDbContext _context;

        public WatchlistRepository(MovieDbContext context)
        {
            _context = context;
        }

        public IQueryable<Watchlist> Watchlists => _context.Wachlists;

        public void AddWatchlist(Watchlist watchlist)
        {
            _context.Wachlists.Add(watchlist);
            _context.SaveChanges();
        }

        public void DeleteWatchlist(long watchlistId)
        {
            var watchlist = _context.Wachlists.FirstOrDefault(w => w.Id == watchlistId);
            if (watchlist != null)
            {
                _context.Wachlists.Remove(watchlist);
                _context.SaveChanges();
            }
        }

        public void UpdateWatchlist(Watchlist watchlist)
        {
            _context.Wachlists.Update(watchlist);
            _context.SaveChanges();
        }
    }
}