using System;
using System.Collections.Generic;
using VideoPlayer.Model;
using Xunit;

namespace VideoPlayer.Tests
{
    public class Video_Should
    {
        private Film film;

        public Video_Should()
        {
            film = new Film()
            {
                CategoriesString = "Akcijski,Avanturistièki,Vestern",
                ListActors = "John Doe,Jura Stublic",
                Length = 100,
                Name = "Test12",
                Year = 2010
            };
        }

        [Fact]
        public void FilmClassTest()
        {
            Assert.NotNull(film);
            Assert.Equal(new List<string>() { "John Doe", "Jura Stublic" }, film.Actors);
            Assert.Equal(new List<Category>() { Category.Akcijski, Category.Avanturistièki, Category.Vestern}, film.Categories);
            Assert.Equal("Test12", film.Name);
        }
    }
}
