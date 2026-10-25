using Microsoft.EntityFrameworkCore;

namespace Assignment1_WebAPI_Part3.Entities
{
    [PrimaryKey(nameof(Patientid), nameof(Doctorid))]
    public class Appointment
    {
        public int Patientid { get; set; }

        public int Doctorid { get; set; }

        public DateTime AppointmentDate { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
