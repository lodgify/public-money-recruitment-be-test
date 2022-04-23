namespace VacationRental.Data
{
    public interface IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        IDictionary<int, TEntity> GetAllEntities();

        TEntity GetEntityById(int id);

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        bool Delete(int id);
    }
}
