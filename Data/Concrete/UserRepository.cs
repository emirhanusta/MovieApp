using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieDbContext _context;

        public UserRepository(MovieDbContext context)
        {
            _context = context;
        }
        public IQueryable<User> Users => _context.Users;

        public void AddUser(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteUser(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateUser(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}