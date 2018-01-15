using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        protected VideoManagerDbContext DbContext { get; }
        public RepositoryBase(VideoManagerDbContext context)
        {
            this.DbContext = context;
        }

        public virtual TEntity Find(int id)
        {
            return this.DbContext.Set<TEntity>()
                .Where(p => p.ID == id)
                .FirstOrDefault();
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public void Add(TEntity model, bool autoSave = false)
        {
            model.DateCreated = DateTime.Now;

            this.DbContext.Set<TEntity>().Add(model);

            if (autoSave)
                this.Save();
        }

        public void Update(TEntity model, bool autoSave = false)
        {
            model.DateModified = DateTime.Now;

            this.DbContext.Entry(model).State = EntityState.Modified;

            if (autoSave)
                this.Save();
        }

        public virtual void Delete(int id, bool autoSave = false)
        {
            var entity = this.DbContext.Set<TEntity>().Find(id);
            this.DbContext.Entry(entity).State = EntityState.Deleted;

            if (autoSave)
                this.Save();
        }
    }
}
