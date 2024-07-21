using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IActorRepository
    {
        IQueryable<Actor> Actors { get; }
        void AddActor(Actor actor);
        void UpdateActor(Actor actor);
        void DeleteActor(Actor actor);
    }
}