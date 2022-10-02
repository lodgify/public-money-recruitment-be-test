namespace VacationRental.Domain.Extensions.Common
{
    public class ConflictException : Exception
	{
		public ConflictException(string message)
			: base(message)
		{ }
	}
}
