using Microsoft.VisualBasic;

namespace Assignment1_WebAPI_Part3.Entities
{
    public class Patient
    {
        public int id { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
