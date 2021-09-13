using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Data
{
    internal class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable
	
        where TEntity : class
	{
		private readonly Context dbContext;
				
		private DbSet<TEntity> dbSet;

		private bool disposed;

		public BaseRepository(Context dbContext)
		{
			this.dbContext = dbContext;
			this.dbSet = dbContext.Set<TEntity>();
		}

		public virtual void Add(TEntity entity)
		{
			this.dbSet.Add(entity);
		}

		public virtual void Delete(TEntity entity)
		{
			this.dbSet.Remove(entity);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.dbContext.Dispose();
				}
			}
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return this.dbSet;
		}

		public virtual TEntity GetById(long id)
		{
			return this.dbSet.Find(new object[] { id });
		}

		public virtual void SaveChanges()
		{
			this.dbContext.SaveChanges();
		}

		public virtual void Update(TEntity entity)
		{            

        }

		public virtual IEnumerable<TEntity> Where(Func<TEntity, bool> parameters)
		{
			return dbSet.Where<TEntity>(parameters);
			
		}
	}
}