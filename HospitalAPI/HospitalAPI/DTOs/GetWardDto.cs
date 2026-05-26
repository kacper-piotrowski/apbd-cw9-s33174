namespace HospitalAPI.DTOs;

public class GetWardDto
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;
}