using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
using Microsoft.AspNetCore.Identity;

namespace MovieApp.Data.Concrete.Context
{
    public static class SeedData
    {
        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            string adminName = "emirhanusta";
            string adminPassword = "Password_123";

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<MovieDbContext>();

            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<Role>>();

            var admin = await userManager.FindByNameAsync(adminName);
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new Role { Name = "User" });
            }
            if (admin == null)
            {

                admin = new User
                {

                    Name = "Emirhan Usta",
                    UserName = adminName,
                    Email = "admin@emirhanusta.com",
                    Image = "admin.jpg",
                    CreatedDate = DateTime.Now
                };

                await userManager.CreateAsync(admin, adminPassword);

                await userManager.AddToRoleAsync(admin, "Admin");
            }

            


        }

        public static void Initialize(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<MovieDbContext>();
            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(
                new Actor { Id = 1, Name = "Leonardo DiCaprio", CreatedDate = DateTime.Now, Biography = "Leonardo Wilhelm DiCaprio is an American actor, producer, and environmentalist. Known for his work in biopics and period films, DiCaprio is the recipient of numerous accolades, including an Academy Award, a British Academy Film Award, and three Golden Globe Awards.", Image = "leonardo-dicaprio.jpg" },
                new Actor { Id = 2, Name = "Tom Hanks", CreatedDate = DateTime.Now, Biography = "Thomas Jeffrey Hanks is an American actor and filmmaker. Known for both his comedic and dramatic roles, Hanks is one of the most popular and recognizable film stars worldwide, and is widely regarded as an American cultural icon.", Image = "tom-hanks.jpg" },
                new Actor { Id = 3, Name = "Johnny Depp", CreatedDate = DateTime.Now, Biography = "John Christopher Depp II is an American actor, producer, and musician. He has been nominated for ten Golden Globe Awards, winning one for Best Actor for his performance of the title role in Sweeney Todd: The Demon Barber of Fleet Street (2008) and has been nominated for three Academy Awards for Best Actor, among other accolades.", Image = "johnny-depp.jpg" },
                new Actor { Id = 4, Name = "Cillian Murphy", CreatedDate = DateTime.Now, Biography = "Cillian Murphy is an Irish actor. He began his career performing as a rock musician. After turning down a record deal, he began his acting career in theatre, and in short and independent films in the late 1990s.", Image = "cillian-murphy.jpg" },
                new Actor { Id = 5, Name = "Tom Hardy", CreatedDate = DateTime.Now, Biography = "Edward Thomas Hardy CBE is an English actor, producer, and former model. After studying acting at the Drama Centre London, he made his film debut in Ridley Scott's Black Hawk Down (2001).", Image = "tom-hardy.jpg" },
                new Actor { Id = 6, Name = "Joseph Gordon-Levitt", CreatedDate = DateTime.Now, Biography = "Joseph Leonard Gordon-Levitt is an American actor, filmmaker, singer, and entrepreneur. As a child, Gordon-Levitt appeared in the films A River Runs Through It, Angels in the Outfield, Holy Matrimony, and 10 Things I Hate About You, and as Tommy Solomon in the TV series 3rd Rock from the Sun.", Image = "joseph-gordon-levitt.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Directors.Any())
                {
                    context.Directors.AddRange(
                new Director { Id = 1, Name = "Christopher Nolan", CreatedDate = DateTime.Now, Biography = "Christopher Edward Nolan CBE is a British-American film director, producer, and screenwriter. His films have grossed over US$7.5 billion worldwide, and he is one of the highest-grossing directors in history.", Image = "christopher-nolan.jpg" },
                new Director { Id = 2, Name = "Steven Spielberg", CreatedDate = DateTime.Now, Biography = "Steven Allan Spielberg is an American film director, producer, and screenwriter. He began his career in the New Hollywood era and is currently the most commercially successful director.", Image = "steven-spielberg.jpg" },
                new Director { Id = 3, Name = "Martin Scorsese", CreatedDate = DateTime.Now, Biography = "Martin Charles Scorsese is an American film director, producer, screenwriter, and actor. One of the major figures of the New Hollywood era, he is widely regarded as one of the greatest directors in the history of cinema.", Image = "martin-scorsese.jpg" },
                new Director { Id = 4, Name = "Quentin Tarantino", CreatedDate = DateTime.Now, Biography = "Quentin Jerome Tarantino is an American film director, screenwriter, producer, and actor. His films are characterized by nonlinear storylines, dark humor, stylized violence, extended dialogue, ensemble casts, references to popular culture, alternate history, and neo-noir.", Image = "quentin-tarantino.jpg" },
                new Director { Id = 5, Name = "David Fincher", CreatedDate = DateTime.Now, Biography = "David Andrew Leo Fincher is an American film director. Known for his psychological thrillers, his films have received 40 nominations at the Academy Awards, including two for him as Best Director.", Image = "david-fincher.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(
                new Genre { Id = 1, Name = "Action", CreatedDate = DateTime.Now, },
                new Genre { Id = 2, Name = "Adventure", CreatedDate = DateTime.Now, },
                new Genre { Id = 3, Name = "Comedy", CreatedDate = DateTime.Now, },
                new Genre { Id = 4, Name = "Crime", CreatedDate = DateTime.Now, },
                new Genre { Id = 5, Name = "Drama", CreatedDate = DateTime.Now, },
                new Genre { Id = 6, Name = "Fantasy", CreatedDate = DateTime.Now, },
                new Genre { Id = 7, Name = "Historical", CreatedDate = DateTime.Now, },
                new Genre { Id = 8, Name = "Horror", CreatedDate = DateTime.Now, },
                new Genre { Id = 9, Name = "Mystery", CreatedDate = DateTime.Now, },
                new Genre { Id = 10, Name = "Romance", CreatedDate = DateTime.Now, }
                    );
                    context.SaveChanges();
                }
                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(
                new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    CreatedDate = DateTime.Now,
                    Description = "Inception is a 2010 science fiction action film written and directed by Christopher Nolan, who also produced the film with Emma Thomas, his wife. The film stars Leonardo DiCaprio as a professional thief who steals information by infiltrating the subconscious of his targets.",
                    Image = "inception.jpg",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    DirectorId = 1,
                    Genres = context.Genres.Where(x => x.Id == 1 || x.Id == 2).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 1 || x.Id == 2).ToList()
                },
                new Movie
                {
                    Id = 2,
                    Title = "Catch Me If You Can",
                    CreatedDate = DateTime.Now,
                    Description = "Catch Me If You Can is a 2002 American biographical crime film directed and produced by Steven Spielberg from a screenplay by Jeff Nathanson. The film stars Leonardo DiCaprio and Tom Hanks, with Christopher Walken, Martin Sheen, and Nathalie Baye in supporting roles.",
                    Image = "catch-me-if-you-can.jpg",
                    ReleaseDate = new DateTime(2002, 12, 25),
                    DirectorId = 2,
                    Genres = context.Genres.Where(x => x.Id == 3).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 1 || x.Id == 2).ToList()
                },
                new Movie
                {
                    Id = 3,
                    Title = "The Terminal",
                    CreatedDate = DateTime.Now,
                    Description = "The Terminal is a 2004 American comedy-drama film co-produced and directed by Steven Spielberg and starring Tom Hanks, Catherine Zeta-Jones, and Stanley Tucci. The film is about an Eastern European",
                    Image = "the-terminal.jpg",
                    ReleaseDate = new DateTime(2004, 6, 18),
                    DirectorId = 2,
                    Genres = context.Genres.Where(x => x.Id == 3).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 2).ToList()
                },
                new Movie
                {
                    Id = 4,
                    Title = "Pirates of the Caribbean: The Curse of the Black Pearl",
                    CreatedDate = DateTime.Now,
                    Description = "Pirates of the Caribbean: The Curse of the Black Pearl is a 2003 American fantasy swashbuckler film directed by Gore Verbinski and the first film in the Pirates of the Caribbean film series.",
                    Image = "pirates-of-the-caribbean.jpg",
                    ReleaseDate = new DateTime(2003, 7, 9),
                    DirectorId = 3,
                    Genres = context.Genres.Where(x => x.Id == 1 || x.Id == 6).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 3).ToList()
                },
                new Movie
                {
                    Id = 5,
                    Title = "In Time",
                    CreatedDate = DateTime.Now,
                    Description = "In Time is a 2011 American science fiction action film written, directed, and produced by Andrew Niccol. Amanda Seyfried and Justin Timberlake star as inhabitants in a society where people stop aging at 25.",
                    Image = "in-time.jpg",
                    ReleaseDate = new DateTime(2011, 10, 28),
                    DirectorId = 4,
                    Genres = context.Genres.Where(x => x.Id == 1 || x.Id == 5).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 3 || x.Id == 4).ToList()
                },
                new Movie
                {
                    Id = 6,
                    Title = "The Dark Knight",
                    CreatedDate = DateTime.Now,
                    Description = "The Dark Knight is a 2008 superhero film directed, produced, and co-written by Christopher Nolan. Based on the DC Comics character Batman, the film is the second installment of Nolan's The Dark Knight Trilogy and a sequel to 2005's Batman Begins, starring Christian Bale and supported by Michael Caine, Heath Ledger, Gary Oldman, Aaron Eckhart, Maggie Gyllenhaal, and Morgan Freeman.",
                    Image = "the-dark-knight.jpg",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    DirectorId = 1,
                    Genres = context.Genres.Where(x => x.Id == 1).ToList(),
                    Actors = context.Actors.Where(x => x.Id == 1).ToList()
                }
                    );
                    context.SaveChanges();
                }
            }

        }

    }

}
