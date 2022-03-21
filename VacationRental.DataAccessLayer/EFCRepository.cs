using VacationRental.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.SqlDataAccess
{
	public class EFCRepository<T> : GIRepository<T> where T : class
	{
		private readonly DbSet<T> dbSet;
		/// The actual context of the database registered at startup
		private readonly DatabaseContext dbContext;
		///DbContext being injected</param>
		public EFCRepository(DatabaseContext dbContext)
		{
			this.dbSet = dbContext.Set<T>();
			this.dbContext = dbContext;
		}
		/// Adds a given model item to its table
		public async Task Add(T item) => await dbSet.AddAsync(item);
		/// Returns an IQueriable of the dbSet that we can use to make queries as no tracking, since it's going to be used only to read registers from the database.
		public IQueryable<T> Query => dbSet.AsNoTracking();
		/// Saves the dbContext, all changes made to the database will be saved.
		public async Task Save() => await dbContext.SaveChangesAsync();
		/// Updates the resource in DB.
		public void Update(T item) => dbSet.Update(item);
	}
}
