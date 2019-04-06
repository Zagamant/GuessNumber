using System.Collections.Generic;
using System.Linq;
using Dal.DataBaseHelper;
using Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repository.DataBase
{
    public class BaseRepository<T> where T : BaseModel
    {
        protected GameContext GameContext { get; set; }
        protected DbSet<T> Entity { get; set; }

        public BaseRepository(GameContext gameContext)
        {
            GameContext = gameContext;
            Entity = GameContext.Set<T>();
        }

        public virtual T Save(T model)
        {
            Entity.Add(model);
            GameContext.SaveChanges();
            return model;
        }

        public List<T> GetAll()
        {
            return Entity.ToList();
        }

        public virtual T Get(long id)
        {
            return Entity.SingleOrDefault(x => x.Id == id);
        }

        public void Remove(long id)
        {
            Remove(Get(id));
        }

        public void Remove(T model)
        {
            Entity.Remove(model);
            GameContext.SaveChanges();
        }
    }
}
