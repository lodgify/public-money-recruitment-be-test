namespace VacationRental.Domain.Primitives
{
    public abstract class BaseDomainModel
    {
        public int Id { get; set; } 

        protected BaseDomainModel()
        {
            Id = 0;
        }
    }
}
