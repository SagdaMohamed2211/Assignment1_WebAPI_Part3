
using Assignment1_WebAPI_Part3.Data;
using Assignment1_WebAPI_Part3.DTOs;
using Assignment1_WebAPI_Part3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DoctorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _context.Doctors
            .AsNoTracking()
            .Select(d => new DoctorReadDto
            {
                Id = d.id,
                Name = d.Name,
                Specialization = d.Specialization
            })
            .ToListAsync();

        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _context.Doctors
            .AsNoTracking()
            .Where(d => d.id == id)
            .Select(d => new DoctorReadDto
            {
                Id = d.id,
                Name = d.Name,
                Specialization = d.Specialization
            })
            .FirstOrDefaultAsync();

        if (doctor == null)
            return NotFound();

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DoctorCreateDto dto)
    {
        var doctor = new Doctor
        {
            Name = dto.Name,
            Specialization = dto.Specialization
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        var result = new DoctorReadDto
        {
            Id = doctor.id,
            Name = doctor.Name,
            Specialization = doctor.Specialization
        };

        return CreatedAtAction(
            nameof(GetById), new { id = doctor.id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id, DoctorUpdateDto dto)
    {
        var doctor = await _context.Doctors.FindAsync(id);

        if (doctor == null)
            return NotFound();

        doctor.Name = dto.Name;
        doctor.Specialization = dto.Specialization;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);

        if (doctor == null)
            return NotFound();

        var hasAppointments = await _context.Appointments
            .AnyAsync(a => a.Doctorid == id);

        if (hasAppointments)
            return Conflict(
                "Cannot delete a doctor who has appointments.");

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
