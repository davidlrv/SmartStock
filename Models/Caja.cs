namespace SmartStock.Models
{
    public class Caja
    {
        public int ID_Caja { get; set; }
        public int NombreCaja { get; set; }
      
        public string Cod_Caja { get; set; }
        
        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }
    public class ResponseDataCaja
    {
        public List<Caja> DATA { get; set; }
    }
}
