namespace HospitalAPI.DTOs;

public class GetPatientDto
{
    public string Pesel { get; set; } = String.Empty;

    public string FirstName { get; set; } = String.Empty;

    public string LastName { get; set; } = String.Empty;

    public int Age { get; set; }

    public bool Sex { get; set; }

    public ICollection<GetAdmissionDto> Admissions { get; set; } = new List<GetAdmissionDto>();

    public virtual ICollection<GetBedAssignmentDto> BedAssignments { get; set; } = new List<GetBedAssignmentDto>();
}