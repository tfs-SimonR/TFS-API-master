using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TFS_API.Factories.IFactories;

namespace TFS_API.Repositorys
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class RepositoryBase<T, TContext> where T : class where TContext : DbContext, new()
    {
        #region Properties
        private TContext _dataContext;
        private readonly IDbSet<T> _dbSet;
        /// <summary>
        /// 
        /// </summary>
        protected IDbFactory<TContext> DbFactory
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        protected TContext DbContext
        {
            get { return _dataContext ?? (_dataContext = this.DbFactory.Init()); }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFactory"></param>
        protected RepositoryBase(IDbFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        #region Implementation
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dataContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbSet.Remove(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault<T>();
        }

        #endregion

    }
}