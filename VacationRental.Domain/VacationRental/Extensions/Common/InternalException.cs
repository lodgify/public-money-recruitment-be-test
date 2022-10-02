namespace VacationRental.Domain.VacationRental.Extensions.Common
{
    public class InternalException : Exception
	{
		public InternalException(string message)
			: base(message)
		{ }
	}
}
