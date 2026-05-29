namespace apbd_2026_cw11.DTOs;

public class AddBedAssignmentDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string BedType { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
}