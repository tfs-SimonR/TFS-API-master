using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using SPARQTestDAL;

namespace TFS_API.Repositorys
{
    public class BaseRepository<TEntityType> where TEntityType : class
    {
        private readonly SparqTestDbContext _context;
        private readonly CCDBContext _ccdbContext;
        private readonly Mi9DBEntities _mi9Context;
        internal DbSet<TEntityType> dbSet;
        protected BaseRepository(SparqTestDbContext context, 
            CCDBContext ccdbContext, 
            Mi9DBEntities mi9Context)
        {
            _context = context;
            _ccdbContext = ccdbContext;
            _mi9Context = mi9Context;
        }

        public virtual IEnumerable<TEntityType> Get(
            Expression<Func<TEntityType, bool>> filter = null,
            Func<IQueryable<TEntityType>, IOrderedQueryable<TEntityType>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntityType> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntityType GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntityType entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntityType entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntityType entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntityType entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}