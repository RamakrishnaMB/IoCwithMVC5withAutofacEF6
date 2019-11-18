using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        T GetById(object id);

        void Add(T entity);
        void Delete(T entity);

        void DeleteAll(List<T> entityList);
        void Update(T entity);
        void UpdateChanges(T entity);
        void SaveChanges();

        void SaveChangesAsync();
        Task SaveChangesTaskAsync();
        IQueryable<T> Fetch(Expression<Func<T, bool>> filter = null);
        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetList(Func<T, bool> where, Expression<Func<T, object>> orderBy, SortOrder sortOrder, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, Expression<Func<T, object>> orderByDescending, params Expression<Func<T, object>>[] navigationProperties);
        DbRawSqlQuery<T> ExecWithStoreProcedure<T>(string sql, params object[] parameters);
        List<TReturn> ExecWithStoreProcedure<TParameters, TReturn>(string storedProcedure, TParameters parameters);
        T Reload(T entity);

        void AddRange(List<T> entityList);
        void ExecSqlCommand(string sql, params object[] parameters);
    }
}
