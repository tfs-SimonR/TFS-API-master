using CashingUpDAL;
using ClickAndCollectDAL;
using Mi9DataAccessLayer;
using PICountDataAccessLayer;
using ProcessedDAL;
using SPARQTestDAL;
using System;
using TFS_API.Models;
using TFS_API.Repositorys.Interfaces;

namespace TFS_API.Repositorys
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly SparqTestDbContext _context;
        private readonly CCDBContext _ccdbContext;
        private readonly Mi9DBEntities _mi9Context;
        private readonly piDBContext _piDbContext;
        private readonly CashDbContext _cashDbContext;
        private readonly ProDbContext _proDbContext;
        private readonly ApplicationDbContext _appContext;

        public UnitOfWork()
        {
            _context = new SparqTestDbContext();
            _ccdbContext = new CCDBContext();
            _mi9Context = new Mi9DBEntities();
            _piDbContext = new piDBContext();
            _cashDbContext = new CashDbContext();
            _proDbContext = new ProDbContext();
            _appContext = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
            CCDBContext.SaveChanges();
            MI9DBContext.SaveChanges();
            PIDbContext.SaveChanges();
            CashDbContext.SaveChanges();
            ProDbContext.SaveChanges();
            AppContext.SaveChanges();

        }

        public ApplicationDbContext AppContext => _appContext;
        /// <summary>
        /// 
        /// </summary>
        public SparqTestDbContext Context => _context;
        /// <summary>
        /// 
        /// </summary>
        public CCDBContext CCDBContext => _ccdbContext;
        /// <summary>
        /// 
        /// </summary>
        public Mi9DBEntities MI9DBContext => _mi9Context;
        /// <summary>
        /// 
        /// </summary>
        public piDBContext PIDbContext => _piDbContext;
        /// <summary>
        /// 
        /// </summary>
        public CashDbContext CashDbContext => _cashDbContext;

        public ProDbContext ProDbContext => _proDbContext;

        public void Dispose()
        {
            _context.Dispose();
            _ccdbContext.Dispose();
            _cashDbContext.Dispose();
            _appContext.Dispose();
        }
    }
}