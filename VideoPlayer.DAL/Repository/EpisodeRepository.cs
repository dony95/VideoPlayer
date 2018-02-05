using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public class EpisodeRepository : RepositoryBase<Episode>, IRepositoryBase<Episode>
    {
        public EpisodeRepository(VideoManagerDbContext context)
           : base(context) { }

        public List<Episode> GetList(IFilmFilter filter)
        {
            return DbContext.Episodes.ToList();
        }

        public virtual List<Episode> GetListForSeriesAndSeason(int seasonID, int seriesID)
        {
            return DbContext.Episodes.Where(e => e.SeasonId == seasonID).Where(e => e.Season.SeriesId == seriesID).ToList();
            
        }
    }
}
