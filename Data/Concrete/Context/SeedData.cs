using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete.Context
{
    public static class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<MovieDbContext>();
            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                new User { Id = 1, Username = "admin", Password = "admin", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) },
                new User { Id = 2, Username = "user", Password = "user", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) }
                    );
                    context.SaveChanges();
                }
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(
                new Actor { Id = 1, Name = "Leonardo DiCaprio", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Biography = "Leonardo Wilhelm DiCaprio is an American actor, producer, and environmentalist. Known for his work in biopics and period films, DiCaprio is the recipient of numerous accolades, including an Academy Award, a British Academy Film Award, and three Golden Globe Awards.", Image = "leonardo-dicaprio.jpg" },
                new Actor { Id = 2, Name = "Tom Hanks", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Biography = "Thomas Jeffrey Hanks is an American actor and filmmaker. Known for both his comedic and dramatic roles, Hanks is one of the most popular and recognizable film stars worldwide, and is widely regarded as an American cultural icon.", Image = "tom-hanks.jpg" },
                new Actor { Id = 3, Name = "Johnny Depp", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Biography = "John Christopher Depp II is an American actor, producer, and musician. He has been nominated for ten Golden Globe Awards, winning one for Best Actor for his performance of the title role in Sweeney Todd: The Demon Barber of Fleet Street (2008) and has been nominated for three Academy Awards for Best Actor, among other accolades.", Image = "johnny-depp.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Directors.Any())
                {
                    context.Directors.AddRange(
                new Director { Id = 1, Name = "Christopher Nolan", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Biography = "Christopher Edward Nolan CBE is a British-American film director, producer, and screenwriter. His films have grossed over US$7.5 billion worldwide, and he is one of the highest-grossing directors in history.", Image = "christopher-nolan.jpg" },
                new Director { Id = 2, Name = "Steven Spielberg", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Biography = "Steven Allan Spielberg is an American film director, producer, and screenwriter. He began his career in the New Hollywood era and is currently the most commercially successful director.", Image = "steven-spielberg.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(
                new Genre { Id = 1, Name = "Action", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) },
                new Genre { Id = 2, Name = "Adventure", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) },
                new Genre { Id = 3, Name = "Comedy", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) }
                    );
                    context.SaveChanges();
                }
                if (!context.Wachlists.Any())
                {
                    context.Wachlists.AddRange(
                new Watchlist { Id = 1, UserId = 1, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) },
                new Watchlist { Id = 2, UserId = 2, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now) }
                    );
                    context.SaveChanges();
                }

                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(
                new Movie { Id = 1, Title = "Inception", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Description = "Inception is a 2010 science fiction action film written and directed by Christopher Nolan, who also produced the film with Emma Thomas, his wife. The film stars Leonardo DiCaprio as a professional thief who steals information by infiltrating the subconscious of his targets.", Image = "inception.jpg", ReleaseDate = new DateTime(2010, 7, 16), DirectorId = 1 },
                new Movie { Id = 2, Title = "Catch Me If You Can", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Description = "Catch Me If You Can is a 2002 American biographical crime film directed and produced by Steven Spielberg from a screenplay by Jeff Nathanson. The film stars Leonardo DiCaprio and Tom Hanks, with Christopher Walken, Martin Sheen, and Nathalie Baye in supporting roles.", Image = "catch-me-if-you-can.jpg", ReleaseDate = new DateTime(2002, 12, 25), DirectorId = 2 },
                new Movie { Id = 3, Title = "The Terminal", CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Description = "The Terminal is a 2004 American comedy-drama film co-produced and directed by Steven Spielberg and starring Tom Hanks, Catherine Zeta-Jones, and Stanley Tucci. The film is about an Eastern European", Image = "the-terminal.jpg", ReleaseDate = new DateTime(2004, 6, 18), DirectorId = 2 }
                    );
                    context.SaveChanges();
                }
                if (!context.Reviews.Any())
                {
                    context.Reviews.AddRange(
                new Review { Id = 1, MovieId = 1, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Rating = 5, Content = "Great movie!", UserId = 1 },
                new Review { Id = 2, MovieId = 1, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Rating = 4, Content = "Awesome!", UserId = 1 },
                new Review { Id = 3, MovieId = 2, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), Rating = 5, Content = "Great movie!", UserId = 2 }
                    );
                    context.SaveChanges();
                }
                if (!context.Likes.Any())
                {
                    context.Likes.AddRange(
                new Like { Id = 1, ReviewId = 1, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), UserId = 1 },
                new Like { Id = 2, ReviewId = 2, CreatedDate = DateOnly.FromDateTime(DateTime.Now), UpdatedDate = DateOnly.FromDateTime(DateTime.Now), UserId = 2 }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}