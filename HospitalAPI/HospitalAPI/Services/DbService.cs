using HospitalAPI.Data;

namespace HospitalAPI.Services;

public class DbService : IDbService
{
    private readonly HospitalDbContext _dbContext;
    public DbService(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}