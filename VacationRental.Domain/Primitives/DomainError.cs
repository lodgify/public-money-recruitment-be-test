namespace VacationRental.Domain.Primitives
{
    public class DomainError
    {
        public string Message { get; set; }
        public int CodeId { get; set; }


        public DomainError(int codeId, string message)
        {
            Message = message;
            CodeId = codeId;
        }
    }
}
