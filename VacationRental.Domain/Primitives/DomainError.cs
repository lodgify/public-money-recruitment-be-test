namespace VacationRental.Domain.Primitives
{
    public class DomainError
    {
        public string Message { get; set; }        

        public DomainError(string message)
        {
            Message = message;            
        }
    }
}
