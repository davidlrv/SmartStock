namespace SmartStock.Models
{
    public class UnidadMedida
    {
        public int? ID_UnidadMedida { get; set; }

        public string Nombre_UnidadMedida { get; set; }

        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataUnidadMedida
    {
        public List<UnidadMedida> DATA { get; set; }
    }
}
