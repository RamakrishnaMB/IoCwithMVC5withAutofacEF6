using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace DataLayer.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SchoolEntities _context;
        public Repository(SchoolEntities context)
        {
            _context = context;
            // temporary fix for entity framework instance failure
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
   
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            if (navigationProperties != null)
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, include) => current.Include(include));

            return dbQuery.AsNoTracking();
        }

        public IQueryable<T> Fetch(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }


        public void DeleteAll(List<T> entityList)
        {
            foreach (var entity in entityList)
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();

            }
        }

        public void Update(T entity)
        {
            var entry = _context.Entry<T>(entity);
            DbSet<T> dbSet = _context.Set<T>();
            var primarykey = dbSet.Create().GetType().GetProperty("Id").GetValue(entity);

            if (entry.State == EntityState.Modified)
            {
                var set = _context.Set<T>();
                T attachedEntity = set.Find(primarykey);

                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }
                else
                {
                    entry.State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }

        public async Task SaveChangesTaskAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateChanges(T entity)
        {
            var entry = _context.Entry<T>(entity);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            if (navigationProperties != null)
                dbQuery = navigationProperties.Aggregate(dbQuery.Where(where).AsQueryable(), (current, include) => current.Include(include));

            return dbQuery.ToList();
        }


        public IList<T> GetList(Func<T, bool> where, Expression<Func<T, object>> orderBy, SortOrder sortOrder, params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            if (navigationProperties != null)
                dbQuery = navigationProperties.Aggregate(dbQuery.Where(where).AsQueryable(), (current, include) => current.Include(include));

            if (orderBy != null)
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    dbQuery = dbQuery.OrderBy(orderBy);
                }
                else if (sortOrder == SortOrder.Descending)
                {
                    dbQuery = dbQuery.OrderByDescending(orderBy);
                }
            }
            return dbQuery.ToList();
        }

        public T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T entity = null;
            IQueryable<T> dbQuery = _context.Set<T>();

            if (navigationProperties != null)
                dbQuery = navigationProperties.Aggregate(dbQuery.AsNoTracking().Where(where).AsQueryable(), (current, include) => current.Include(include));



            entity = dbQuery.SingleOrDefault();

            return entity;
        }

        public T GetSingle(Func<T, bool> where, Expression<Func<T, object>> orderByDescending, params Expression<Func<T, object>>[] navigationProperties)
        {
            T entity = null;
            IQueryable<T> dbQuery = _context.Set<T>();

            if (navigationProperties != null)
                dbQuery = navigationProperties.Aggregate(dbQuery.AsNoTracking().Where(where).AsQueryable(), (current, include) => current.Include(include));

            if (orderByDescending != null)
                dbQuery = dbQuery.OrderByDescending(orderByDescending);

            entity = dbQuery.SingleOrDefault();

            return entity;
        }

        public DbRawSqlQuery<T> ExecWithStoreProcedure<T>(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters);
        }

        public List<TReturn> ExecWithStoreProcedure<TParameters, TReturn>(string storedProcedure, TParameters parameters)
        {
            var procedureParameters = new Dictionary<string, object>();
            var properties = parameters.GetType().GetProperties();
            var sqlParameters = new List<object>();

            foreach (var property in properties)
            {
                var value = property.GetValue(parameters);
                var name = property.Name;

                procedureParameters.Add(name, value);

                sqlParameters.Add(new SqlParameter(name, value));
            }

            var keys = procedureParameters.Select(p => string.Format("@{0}", p.Key)).ToList();
            var parms = string.Join(", ", keys.ToArray());

            return _context.Database.SqlQuery<TReturn>(storedProcedure + " " + parms, sqlParameters.ToArray()).ToList();
        }

        public T Reload(T entity)
        {
            _context.Entry(entity).Reload();
            return entity;
        }


        public void AddRange(List<T> entityList)
        {
            _context.Set<T>().AddRange(entityList);
            _context.SaveChanges();
        }

        public void ExecSqlCommand(string sql, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(sql, parameters);
        }

    }
}
