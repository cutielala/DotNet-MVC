using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbset;

        public Repository(ApplicationDBContext db)
        {
            _db= db;  
            //_db.Products.Include(u=>u.Category).Include(u=>u.CoverType);    
            this.dbset = _db.Set<T>();  
        }

        public void Add(T entity)
        {
            dbset.Add(entity);  
        }
        //includeProp = "Category , CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach(var property in includeProperties.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);    
                } 
                
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked=true)
        {
            IQueryable<T> query;
            
            if (tracked)
            {
                query = dbset; 
            }
            else
            {
                query = dbset.AsNoTracking();
            }
            
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);   
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }

}
