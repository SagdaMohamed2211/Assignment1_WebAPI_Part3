
using Assignment1_WebAPI_Part3.Data;
using Assignment1_WebAPI_Part3.DTOs;
using Assignment1_WebAPI_Part3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _context.Patients
            .AsNoTracking()
            .Select(p => new PatientReadDto
            {
                Id = p.id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth
            })
            .ToListAsync();

        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .Where(p => p.id == id)
            .Select(p => new PatientReadDto
            {
                Id = p.id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth
            })
            .FirstOrDefaultAsync();

        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PatientCreateDto dto)
    {
        var patient = new Patient
        {
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        var result = new PatientReadDto
        {
            Id = patient.id,
            Name = patient.Name,
            DateOfBirth = patient.DateOfBirth
        };

        return CreatedAtAction(
            nameof(GetById), new { id = patient.id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id, PatientUpdateDto dto)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null)
            return NotFound();

        patient.Name = dto.Name;
        patient.DateOfBirth = dto.DateOfBirth;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null)
            return NotFound();

        var hasAppointments = await _context.Appointments
            .AnyAsync(a => a.Patientid == id);

        if (hasAppointments)
            return Conflict(
                "Cannot delete a patient who has appointments.");

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
