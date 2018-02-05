using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VideoPlayer.Controllers.API;
using VideoPlayer.DAL;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;
using Xunit;

namespace VideoPlayer.Tests
{
    public class Film_API_Controller_Should
    {
        DbContextOptions<VideoManagerDbContext> _dbContextOptions;

        public Film_API_Controller_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<VideoManagerDbContext>()
                            .UseInMemoryDatabase(databaseName: "Test_database")
                            .Options;
        }

        [Fact]
        public void PostFilm()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var filmAPI = new FilmController(new FilmRepository(context));
                for(int i = 0; i < 5; i++)
                {
                    Film film = new Film()
                    {
                        CategoriesString = "Akcijski,Avanturistički,Vestern",
                        ListActors = "John Doe,Jura Stublic",
                        Length = 100 + i,
                        Name = "Test12" + i,
                        Year = 2010 + i
                    };
                    var result = filmAPI.Post(film);
                    var badRequest = result as BadRequestObjectResult;
                    var createdResult = result as CreatedAtActionResult;

                    Assert.Null(badRequest);
                    Assert.Equal(film, createdResult.Value);
                }
            }
        }

        [Fact]
        public void GetAllFilms()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                for (int i = 0; i < 5; i++)
                {
                    Film film = new Film()
                    {
                        CategoriesString = "Akcijski,Avanturistički,Vestern",
                        ListActors = "John Doe,Jura Stublic",
                        Length = 100 + i,
                        Name = "Test12" + i,
                        Year = 2010 + i
                    };
                    var result = FilmAPI.Post(film);
                }
            }

            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                var result = FilmAPI.Get();

                Assert.NotNull(result);

                var filmList = (result as IEnumerable<Film>).ToList();

                Assert.NotNull(filmList);
                Assert.Equal(5, filmList.Count);
                Assert.Equal(2014, filmList[0].Year);
            }
        }

        [Fact]
        public void DeleteFilm()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var filmAPI = new FilmController(new FilmRepository(context));
                var film = new Film()
                {
                    CategoriesString = "Akcijski,Avanturistički,Vestern",
                    ListActors = "John Doe,Jura Stublic",
                    Length = 100,
                    Name = "Test12",
                    Year = 2010
                };
                filmAPI.Put(1, film);
            }
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var filmAPI = new FilmController(new FilmRepository(context));
                var result = filmAPI.Delete(1);
                var okResult = result as OkObjectResult;

                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                var film = okResult.Value as Film;
                Assert.NotNull(film);
                Assert.Equal("Test12", film.Name);
            }

            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var filmAPI = new FilmController(new FilmRepository(context));
                var result = filmAPI.Delete(1);
                var notFoundResult = result as NotFoundResult;

                Assert.NotNull(notFoundResult);
                Assert.Equal(404, notFoundResult.StatusCode);
            }

        }

        [Fact]
        public void GetOneFilm()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                Film film = new Film()
                {
                    CategoriesString = "Akcijski,Avanturistički,Vestern",
                    ListActors = "John Doe,Jura Stublic",
                    Length = 100,
                    Name = "TestJedanFilm",
                    Year = 2010
                };
                var result = FilmAPI.Post(film);
            }

            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                var result = FilmAPI.Get(11);
                var okResult = result as OkObjectResult;

                Assert.NotNull(result);
                Assert.Equal(200, okResult.StatusCode);

                var film = okResult.Value as Film;
                Assert.NotNull(film);
                Assert.Equal("TestJedanFilm", film.Name);
            }
        }

        [Fact]
        public void PutFilm()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                Film film = new Film()
                {
                    CategoriesString = "Akcijski,Avanturistički,Vestern",
                    ListActors = "Ivan Cesar, Jura Stublic",
                    Length = 110,
                    Name = "Put film",
                    Year = 2010
                };
                var result = FilmAPI.Put(2, film);
            }

            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var FilmAPI = new FilmController(new FilmRepository(context));
                var result = FilmAPI.Get(2);
                var okResult = result as OkObjectResult;

                Assert.NotNull(result);
                Assert.Equal(200, okResult.StatusCode);

                var film = okResult.Value as Film;
                Assert.NotNull(film);
                Assert.Equal("Put film", film.Name);
                Assert.Equal("Ivan Cesar, Jura Stublic", film.ListActors);
            }
        }

        }
}
