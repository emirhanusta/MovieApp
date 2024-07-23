using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void AddUser(User entity);
        void DeleteUser(User entity);
        void UpdateUser(User entity);
    }
}