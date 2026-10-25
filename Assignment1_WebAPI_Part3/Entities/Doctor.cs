namespace Assignment1_WebAPI_Part3.Entities
{
    public class Doctor
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Specialization { get; set; }


        public ICollection<Appointment> Appointments { get; set; }
    }
}
