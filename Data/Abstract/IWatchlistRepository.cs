using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IWatchlistRepository
    {
        IQueryable<Watchlist> Watchlists { get; }

        void AddWatchlist(Watchlist watchlist);
        void DeleteWatchlist(Watchlist watchlist);
        void UpdateWatchlist(Watchlist watchlist);
    }
}