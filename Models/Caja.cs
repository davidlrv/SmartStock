namespace SmartStock.Models
{
    public class Caja
    {
        public int ID_Caja { get; set; }
        public int ID_Usuario { get; set; }
        public string? Usuario_Apertura { get; set; }
        public string? Usuario_Cierre { get; set; }
        public string Cod_Caja { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public double Monto_Apertura { get; set; }
        public double Ingresos { get; set; }
        public double Devoluciones { get; set; }
        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }
    public class ResponseDataCaja
    {
        public List<Caja> DATA { get; set; }
    }
}
