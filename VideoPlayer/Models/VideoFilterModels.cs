using VideoPlayer.DAL.Repository;
using VideoPlayer.Model;

namespace VideoPlayer.Models
{
    public class FilmFilterModel : IFilmFilter
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public Category Category { get; set; }
    }
    public class CartoonFilterModel : ICartoonFilter
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public Category Category { get; set; }

        public Language Language { get; set; }
    }
}
