using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Persistance.Interfaces;

namespace VacationRental.Persistance.Repository
{
    public class VacationRentalRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        protected readonly DbContext Context;

        public VacationRentalRepository(ApplicationDbContext dataContext)
        {
            DbSet = dataContext.Set<T>();
            Context = dataContext;
        }

        #region IRepository<T> Members

        public void Insert(T entity)
        {
            DbSet.Add(entity);
            Save();
        }

        public async Task InsertAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            SaveAsync();
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
            Save();
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        private void Save()
        {
            Context.SaveChanges();
        }

        private async void SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        #endregion
    }

    
}
