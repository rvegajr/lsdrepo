using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using LaserSportDataObjects;
using System.Linq.Expressions;

namespace LaserSportDataAPI.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        //Thank you!! http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        internal PetaPoco.Database _db = new PetaPoco.Database("LSREPConn");
        protected PetaPoco.Database db {
            get { return _db; }
            set { _db = value; }
        }
        private void GenericRepositoryInit()
        {
            _SQL = _SQL.Replace("%TABLE_NAME%", _TableName);
            if (_SQL_GETALL.Length == 0) _SQL_GETALL = _SQL;
            if (_SQL_GETBYID.Length == 0) _SQL_GETBYID = _SQL + " WHERE " + _IdColumnName + "=@0";
        }
        public GenericRepository(string TableName)
        {
            _TableName = TableName;
            GenericRepositoryInit();
        }
        public GenericRepository(string TableName, string idColumn)
        {
            _TableName = TableName;
            _IdColumnName = idColumn;
            GenericRepositoryInit();
        }
        protected  string _SQL = "SELECT * FROM %TABLE_NAME% ";
        protected  string _SQL_GETALL = "";
        protected  string _SQL_GETBYID = "";
        protected  string _TableName = "";
        protected  string _IdColumnName = "id";
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", params object[] args )
        {

            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            var lst = db.Fetch<TEntity>(_SQL_GETALL, args);

            IQueryable<TEntity> query = lst.AsQueryable();

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

        public virtual TEntity GetByID(params object[] args)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            return db.SingleOrDefault<TEntity>(_SQL_GETBYID, args);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            db.Insert(_TableName, _IdColumnName, entity);
            return entity;
        }

        public virtual int Delete(object id)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            return db.Delete(_TableName, _IdColumnName, "", id);
        }

        public virtual int Delete(TEntity entityToDelete)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            return db.Delete(_TableName, _IdColumnName, entityToDelete);
        }

        public virtual int Update(TEntity entityToUpdate)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            return db.Update(_TableName, _IdColumnName, entityToUpdate);
        }
    }

    public class GenericRepositoryReadOnly<TEntity> where TEntity : class
    {
        //Thank you!! http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        internal PetaPoco.Database _db = new PetaPoco.Database("LSREPConn");
        protected PetaPoco.Database db
        {
            get { return _db; }
            set { _db = value; }
        }
        private void GenericRepositoryInit()
        {
            _SQL = _SQL.Replace("%TABLE_NAME%", _TableName);
            if (_SQL_GETALL.Length == 0) _SQL_GETALL = _SQL;
            if (_SQL_GETBYID.Length == 0) _SQL_GETBYID = _SQL + " WHERE " + _IdColumnName + "=@0";
        }
        public GenericRepositoryReadOnly(string TableName)
        {
            _TableName = TableName;
            GenericRepositoryInit();
        }
        public GenericRepositoryReadOnly(string TableName, string idColumn)
        {
            _TableName = TableName;
            _IdColumnName = idColumn;
            GenericRepositoryInit();
        }
        protected string _SQL = "SELECT * FROM %TABLE_NAME% ";
        protected string _SQL_GETALL = "";
        protected string _SQL_GETBYID = "";
        protected string _TableName = "";
        protected string _IdColumnName = "id";
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", params object[] args)
        {

            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            var lst = db.Fetch<TEntity>(_SQL_GETALL, args);

            IQueryable<TEntity> query = lst.AsQueryable();

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

        public virtual TEntity GetByID(params object[] args)
        {
            if (_TableName.Length == 0) throw new Exception("Must set table name before calling this method");
            return db.SingleOrDefault<TEntity>(_SQL_GETBYID, args);
        }
    }
}