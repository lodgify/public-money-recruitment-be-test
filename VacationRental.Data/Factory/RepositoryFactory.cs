using Microsoft.Extensions.Configuration;
namespace VacationRental.Data
{
    public static class RepositoryFactory
	{
        public static IRepository<T> CreateRepository<T>(IConfiguration configuration)
		where T : class
		{
			return new BaseRepository<T>(new Context(configuration));
		}

        public static Context GetContext(IConfiguration configuration)      
        {
            return new Context(configuration);
        }
    }
}