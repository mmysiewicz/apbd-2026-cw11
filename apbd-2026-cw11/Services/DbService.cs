using apbd_2026_cw11.Data;
using apbd_2026_cw11.DTOs;
using apbd_2026_cw11.Exceptions;
using apbd_2026_cw11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_2026_cw11.Services;

public class DbService : IDbService
{
    private readonly DatabaseFirstDbContext _context;
    public DbService(DatabaseFirstDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetPatientsDto>> GetPatients(string? search)
    {

        var pattern = $"%{search}%";

        var res = _context.Patients
            .Where(e => string.IsNullOrWhiteSpace(search) ||
                        EF.Functions.Like(e.FirstName, pattern) ||
                        EF.Functions.Like(e.LastName, pattern))
            .Select(e => new GetPatientsDto()
            {
                Pesel = e.Pesel,
                FirstName = e.FirstName,
                Age = e.Age,
                Sex = e.Sex.ToString(),
                Admissions = e.Admissions.Select(e => new GetAdmissionsDetailsDto()
                {
                    Id = e.Id,
                    AdmissionDate = e.AdmissionDate,
                    DischargeDate = e.DischargeDate,
                    Ward = new GetWardDetailsDto()
                    {
                        Id = e.Ward.Id,
                        Name = e.Ward.Name,
                        Description = e.Ward.Description
                    }
                }),
                BedAssignments = e.BedAssignments.Select(e => new GetBedAssignmentsDetailsDto()
                {
                    Id = e.Id,
                    From = e.From,
                    To = e.To,
                    Bed = new GetBedDetailsDto()
                    {
                        Id = e.Bed.Id,
                        BedType = new GetBedTypeDetailsDto()
                        {
                            Id = e.Bed.BedType.Id,
                            Name = e.Bed.BedType.Name,
                            Description = e.Bed.BedType.Description
                        }
                    },
                    Room = new GetRoomDetailsDto()
                    {
                        Id = e.Bed.Room.Id,
                        HasTv = e.Bed.Room.HasTv,
                        Ward = new GetWardDetailsDto()
                        {
                            Id = e.Bed.Room.Ward.Id,
                            Name = e.Bed.Room.Ward.Name,
                            Description = e.Bed.Room.Ward.Description
                        }
                    }
                })
            });

        if (res == null)
        {
            throw new NotFoundException();
        }

        return res;
    }

    public async Task AddBedAssignmentAsync(string pesel, AddBedAssignmentDto addBedAssignmentDto)
    {
        var anyPatient = await _context.Patients.AnyAsync(e => e.Pesel == pesel);
        if (!anyPatient)
        {
            throw new NotFoundException($"Patient with pesel {pesel} not exists.");
        }
        
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var anyWard = await _context.Wards.AnyAsync(w => w.Name == addBedAssignmentDto.Ward);
            if (!anyWard)
            {
                throw new NotFoundException($"Ward {addBedAssignmentDto.Ward} not exists.");
            }
            
            var anyBedType = await _context.BedTypes.AnyAsync(bt => bt.Name == addBedAssignmentDto.BedType);
            if (!anyBedType)
            {
                throw new NotFoundException($"BedType {addBedAssignmentDto.BedType} not exists.");
            }

            var bed = await _context.Beds
                .Where(b => b.Room.Ward.Name == addBedAssignmentDto.Ward
                            && b.BedType.Name == addBedAssignmentDto.BedType)
                .Where(b => !b.BedAssignments.Any(ba =>
                    ba.From < addBedAssignmentDto.To
                    && ba.To > addBedAssignmentDto.From))
                .FirstOrDefaultAsync();

            if (bed == null)
            {
                throw new NotFoundException($"Bed with {addBedAssignmentDto.BedType} in Ward {addBedAssignmentDto.Ward} not exists in this period of time.");
            }

            var bedAssignment = new BedAssignment()
            {
                PatientPesel = pesel,
                BedId = bed.Id,
                From = addBedAssignmentDto.From,
                To = addBedAssignmentDto.To
            };
            
            await _context.BedAssignments.AddAsync(bedAssignment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        } catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}