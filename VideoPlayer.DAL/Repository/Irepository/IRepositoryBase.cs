using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayer.DAL.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        List<TEntity> GetList(IFilmFilter filter);
        TEntity Find(int id);
        void Save();
        void Add(TEntity model, bool autoSave = false);
        void Update(TEntity model, bool autoSave = false);
        void Delete(int id, bool autoSave = false);
    }
}
