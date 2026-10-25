namespace Assignment1_WebAPI_Part3.DTOs
{

    public class AppointmentReadDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class AppointmentCreateDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
    }

}
