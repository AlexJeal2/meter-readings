
namespace MeterReadings.Data.Repositories
{
    public interface IRepositoryBase
    {
        Task<int> SaveChangesAsync();
    }
}