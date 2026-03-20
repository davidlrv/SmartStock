namespace SmartStock.Models
{
    public class Tipo
    {
        public int? ID_Tipo { get; set; }
        public string Nombre_Tipo { get; set; }
       
        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataTipo
    {
        public List<Tipo>? DATA { get; set; }
    }
}
