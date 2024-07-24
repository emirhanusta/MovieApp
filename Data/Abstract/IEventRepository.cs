using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IEventRepository
    {
        IQueryable<Event> Events { get; }
        void AddEvent(Event entity);
        void UpdateEvent(Event entity);
        void DeleteEvent(Event entity);
    }
}