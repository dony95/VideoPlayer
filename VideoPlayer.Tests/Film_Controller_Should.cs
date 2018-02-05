using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Controllers;
using VideoPlayer.DAL;
using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;
using Xunit;

namespace VideoPlayer.Tests
{
    public class Film_Controller_Should
    {
        private DbContextOptions<VideoManagerDbContext> _dbContextOptions;

        public Film_Controller_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<VideoManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void EmptyIndexTest()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var mockRepo = new Mock<FilmRepository>();
                mockRepo.Setup(repo => repo.GetList(null)).Returns((GetTestFilms()));
                var filmController = new FilmController(mockRepo.Object);

                var result = filmController.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Film>>(
                    viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());

            }
        }

        [Fact]
        public void IndexWithFilterTest()
        {
            using (var context = new VideoManagerDbContext(_dbContextOptions))
            {
                var mockRepo = new Mock<FilmRepository>();
                mockRepo.Setup(repo => repo.GetList(null)).Returns((GetTestFilms()));
                var filmController = new FilmController(mockRepo.Object);

                var result = filmController.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Film>>(
                    viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());

            }
        }

        private List<Film> GetTestFilms()
        {
            return new List<Film>()
            {
                new Film()
                {
                    CategoriesString = "Akcijski,Avanturistički,Vestern",
                    ListActors = "John Doe,Jura Stublic",
                    Length = 100,
                    Name = "Film 1",
                    Year = 2010
                },
                new Film()
                {
                    CategoriesString = "Akcijski,Avanturistički,Vestern",
                    ListActors = "Ivan Cesar,Ivan Saric",
                    Length = 101,
                    Name = "Film 2",
                    Year = 2011
                }
            };
        }
    }
}
