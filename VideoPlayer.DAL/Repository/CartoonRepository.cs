using System.Collections.Generic;
using System.Linq;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public class CartoonRepository : RepositoryBase<Cartoon>
    {
        public CartoonRepository(VideoManagerDbContext context)
            : base(context) { }

        public List<Cartoon> GetList(ICartoonFilter filter)
        {
            var cartoonsQuery = this.DbContext.Cartoons
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter?.Name))
                cartoonsQuery = cartoonsQuery
                    .Where(v => v.Name.ToLower().Contains(filter.Name.ToLower()) || v.Name_ENG.ToLower().Contains(filter.Name.ToLower()));

            if (filter != null && filter.Year != 0)
                cartoonsQuery = cartoonsQuery
                    .Where(v => v.Year == filter.Year);

            if (filter?.Language != null && filter?.Language != 0)
                cartoonsQuery = cartoonsQuery
                    .Where(v => v.Language.Equals(filter.Language));

            if (filter?.Category != null && filter?.Category != 0)
            {
                List<Cartoon> retVal = new List<Cartoon>();
                foreach (Cartoon v in cartoonsQuery)
                    if (v.Categories.Contains(filter.Category))
                        retVal.Add(v);
                return retVal.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
            }

            return cartoonsQuery.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
        }
    }
}
