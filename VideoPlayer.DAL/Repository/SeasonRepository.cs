using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public class SeasonRepository : RepositoryBase<Season>, IRepositoryBase<Season>
    {
        public SeasonRepository(VideoManagerDbContext context)
           : base(context) { }

        public List<Season> GetList(IFilmFilter filter)
        {
            return DbContext.Seasons.Include(e => e.Episodes).ToList();
        }

        public virtual List<Season> GetListForSeries(int seriesID)
        {
            return DbContext.Seasons.Include(e => e.Episodes).Where(s => s.SeriesId == seriesID).ToList();

        }
    }
}
