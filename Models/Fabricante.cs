namespace SmartStock.Models
{
    public class Fabricante
    {
        public int? ID_Fabricante { get; set; }

        public string Nombre_Fabricante { get; set; }

        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataFabricante
    {
        public List<Fabricante> DATA { get; set; }
    }
}
