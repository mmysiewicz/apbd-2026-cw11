namespace apbd_2026_cw11.DTOs;

public class GetPatientsDto
{
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sex  { get; set; } = string.Empty;
    public IEnumerable<GetAdmissionsDetailsDto> Admissions { get; set; } = [];
    public IEnumerable<GetBedAssignmentsDetailsDto> BedAssignments { get; set; } = [];

}

public class GetAdmissionsDetailsDto
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public GetWardDetailsDto Ward { get; set; }
    
}

public class GetWardDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string Description { get; set; } =  string.Empty;
}

public class GetBedAssignmentsDetailsDto
{
    public int Id { get; set; }
    public DateTime From  { get; set; }
    public DateTime? To { get; set; }
    public GetBedDetailsDto Bed { get; set; }
    public GetRoomDetailsDto Room { get; set; }
}

public class GetBedDetailsDto
{
    public int Id { get; set; }
    public GetBedTypeDetailsDto BedType { get; set; }
}

public class GetBedTypeDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class GetRoomDetailsDto
{
    public string Id { get; set; } = string.Empty;
    public bool HasTv { get; set; }
    public GetWardDetailsDto Ward { get; set; }
}