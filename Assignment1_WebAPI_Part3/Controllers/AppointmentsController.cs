
using Assignment1_WebAPI_Part3.Data;
using Assignment1_WebAPI_Part3.DTOs;
using Assignment1_WebAPI_Part3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _context.Appointments
            .AsNoTracking()
            .Select(a => new AppointmentReadDto
            {
                PatientId = a.Patientid,
                DoctorId = a.Doctorid,
                AppointmentDate = a.AppointmentDate
            })
            .ToListAsync();

        return Ok(appointments);
    }

    [HttpGet("{patientId}/{doctorId}")]
    public async Task<IActionResult> GetById(
        int patientId, int doctorId)
    {
        var appointment = await _context.Appointments
            .AsNoTracking()
            .Where(a => a.Patientid == patientId
                     && a.Doctorid == doctorId)
            .Select(a => new AppointmentReadDto
            {
                PatientId = a.Patientid,
                DoctorId = a.Doctorid,
                AppointmentDate = a.AppointmentDate
            })
            .FirstOrDefaultAsync();

        if (appointment == null)
            return NotFound();

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        AppointmentCreateDto dto)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.id == dto.PatientId);

        var doctorExists = await _context.Doctors
            .AnyAsync(d => d.id == dto.DoctorId);

        if (!patientExists || !doctorExists)
            return BadRequest(
                "Patient or doctor does not exist.");

        var appointment = new Appointment
        {
            Patientid = dto.PatientId,
            Doctorid = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var result = new AppointmentReadDto
        {
            PatientId = appointment.Patientid,
            DoctorId = appointment.Doctorid,
            AppointmentDate = appointment.AppointmentDate
        };

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                patientId = appointment.Patientid,
                doctorId = appointment.Doctorid
            },
            result);
    }

    [HttpPut("{patientId}/{doctorId}")]
    public async Task<IActionResult> Update(
        int patientId,
        int doctorId,
        AppointmentUpdateDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(
            new object[] { patientId, doctorId });

        if (appointment == null)
            return NotFound();

        appointment.AppointmentDate = dto.AppointmentDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{patientId}/{doctorId}")]
    public async Task<IActionResult> Delete(
        int patientId, int doctorId)
    {
        var appointment = await _context.Appointments.FindAsync(
            new object[] { patientId, doctorId });

        if (appointment == null)
            return NotFound();

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
