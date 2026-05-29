using apbd_2026_cw11.DTOs;

namespace apbd_2026_cw11.Services;

public interface IDbService
{
    Task<IEnumerable<GetPatientsDto>> GetPatients(string? search);
    Task AddBedAssignmentAsync(string pesel, AddBedAssignmentDto addBedAssignmentDto);
}