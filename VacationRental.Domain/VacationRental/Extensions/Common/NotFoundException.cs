namespace VacationRental.Domain.Extensions.Common
{
    public class NotFoundException : Exception
	{
		public NotFoundException(string message)
			: base(message)
		{ }
	}
}
