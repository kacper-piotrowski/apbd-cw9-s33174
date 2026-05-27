using HospitalAPI.DTOs;

namespace HospitalAPI.Services;

public interface IDbService
{
    Task<IEnumerable<GetPatientDto>> GetPatientsWithSearchAsync(string? search);
}