using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Domain
{
	public interface GIRepository<T>
	{
		/// This function will implement adding a register to the database
		Task Add(T entity);
		/// This function will implement returning an IQueryable that can be used to make querys to the database.
		IQueryable<T> Query { get; }
		/// This function will implement saving the changes done.
		Task Save();

		/// This function will implement updating a register to the database
		void Update(T entity);

	}
}
