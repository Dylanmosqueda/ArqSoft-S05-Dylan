namespace CitasApp.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }

        public string Motivo { get; set; }

        public string Estado { get; set; }

    }
}
