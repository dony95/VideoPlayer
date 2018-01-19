using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public class FilmRepository : RepositoryBase<Film>, IRepositoryBase<Film>
    {
        public FilmRepository(VideoManagerDbContext context)
            : base(context) { }

        public List<Film> GetList(IFilmFilter filter)
        {
            var videosQuery = this.DbContext.Films
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter?.Name))
                videosQuery = videosQuery
                    .Where(v => v.Name.ToLower().Contains(filter.Name.ToLower()) || v.Name_ENG.ToLower().Contains(filter.Name.ToLower()));

            if (filter != null && filter.Year != 0)
                videosQuery = videosQuery
                    .Where(v => v.Year == filter.Year);

            if (filter?.Category != null && filter?.Category != 0)
            {
                List<Film> retVal = new List<Film>();
                foreach (Film v in videosQuery)
                    if (v.Categories.Contains(filter.Category))
                        retVal.Add(v);
                return retVal.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
            }

            return videosQuery.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
        }
    }
}
