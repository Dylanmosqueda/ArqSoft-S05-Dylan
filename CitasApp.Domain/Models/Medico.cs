namespace CitasApp.Domain.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; } = string.Empty;
        public string Especialidad { get; set; }

        public string NumeroLicencia { get; set; }
    }
}
