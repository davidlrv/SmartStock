

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SmartStock.Models
{
    public class Bodega
    {

        public int? ID_Bodega { get; set; }
        public string Direccion { get; set; }
        public string Cod_Bodega { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataBodega
    {
        public List<Bodega> DATA { get; set; }
    }

    

}
