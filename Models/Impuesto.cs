namespace SmartStock.Models
{
    public class Impuestos
    {
        public int ID_Impuesto { get; set; }

        public string Nombre_Impuesto { get; set; }

        public decimal Porcentaje { get; set; }

        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataImpuestos
    {
        public List<Impuestos> DATA { get; set; }
    }
}