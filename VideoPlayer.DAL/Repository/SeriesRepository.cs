using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public class SeriesRepository : RepositoryBase<Series>, IRepositoryBase<Series>
    {
        public SeriesRepository(VideoManagerDbContext context)
            : base(context) { }

        public List<Series> GetList(IFilmFilter filter)
        {
            var seriesQuery = this.DbContext.Series
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter?.Name))
                seriesQuery = seriesQuery
                    .Where(v => v.Name.ToLower().Contains(filter.Name.ToLower()) || v.Name_ENG.ToLower().Contains(filter.Name.ToLower()));

            if (filter != null && filter.Year != 0)
                seriesQuery = seriesQuery
                    .Where(v => v.Year == filter.Year);

            if (filter?.Category != null && filter?.Category != 0)
            {
                List<Series> retVal = new List<Series>();
                foreach (Series v in seriesQuery)
                    if (v.Categories.Contains(filter.Category))
                        retVal.Add(v);
                return retVal.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
            }

            return seriesQuery.OrderByDescending(v => v.Year).ThenBy(v => v.Name).ToList();
        }

        public Episode FindEpisode(int value)
        {
            throw new NotImplementedException();
        }

        public void AddSeason(Season season, int seriesID, bool autoSave = false)
        {
            season.DateCreated = DateTime.Now;
            var series = this.DbContext.Series.Find(seriesID);
            if (series.Seasons == null)
                series.Seasons = new List<Season>();
            series.Seasons.Add(season);
            series.DateModified = DateTime.Now;

            this.DbContext.Entry(series).State = EntityState.Modified;

            if (autoSave)
                this.Save();
        }




    }
}
