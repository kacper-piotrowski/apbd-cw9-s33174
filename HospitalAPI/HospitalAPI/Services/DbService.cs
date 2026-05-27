using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Exceptions;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Services;

public class DbService : IDbService
{
    private readonly HospitalDbContext _dbContext;
    public DbService(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GetPatientDto>> GetPatientsWithSearchAsync(string? search)
    {
        var query = _dbContext.Patients.AsQueryable();

        if (search != null)
        {
            query = query.Where(p=>p.FirstName.Contains(search) || p.LastName.Contains(search));
        }

        var result = await query.Select(p => new GetPatientDto()
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = p.Age,
            Sex = p.Sex,
            Admissions = p.Admissions.Select(a => new GetAdmissionDto()
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new GetWardDto()
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            BedAssignments = p.BedAssignments.Select(b => new GetBedAssignmentDto()
            {
                Id = b.Id,
                From = b.From,
                To = b.To,
                Bed = new GetBedDto()
                {
                    Id = b.Bed.Id,
                    BedType = new GetBedTypeDto()
                    {
                        Id = b.Bed.BedType.Id,
                        Name = b.Bed.BedType.Name,
                        Description = b.Bed.BedType.Description
                    },
                    Room = new GetRoomDto()
                    {
                        Id = b.Bed.Room.Id,
                        HasTv = b.Bed.Room.HasTv,
                        Ward = new GetWardDto()
                        {
                            Id = b.Bed.Room.Ward.Id,
                            Name = b.Bed.Room.Ward.Name,
                            Description = b.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()
        }).ToListAsync();
        return result;
    }

    public async Task CreateBedAssignmentAsync(string pesel, CreateBedAssignmentDto dto)
    {
        var patientCheck = await _dbContext.Patients.AnyAsync(p=> p.Pesel== pesel);
        if (!patientCheck)
        {
            throw new NotFoundException("Nie znaleziono pacjenta!");
        }
        
        var bedCheck = await _dbContext.Beds.Where(b=> b.BedType.Name == dto.BedType && b.Room.Ward.Name == dto.Ward)
            .Where(b=> !b.BedAssignments.Any(ba =>(dto.To == null || ba.From < dto.To) &&(ba.To == null || ba.To > dto.From))).FirstOrDefaultAsync();

        if (bedCheck == null)
        {
            throw new NotFoundException("Nie ma takiego wolnego łóżka!");
        }

        var bedAssignment = new BedAssignment()
        {
            PatientPesel = pesel,
            BedId = bedCheck.Id,
            From = dto.From,
            To = dto.To,
        };
        await _dbContext.BedAssignments.AddAsync(bedAssignment);
        await _dbContext.SaveChangesAsync();
    }
}