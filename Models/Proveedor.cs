

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SmartStock.Models
{
    public class Proveedor
    {
        public int? ID_Proveedores { get; set; }
        public string Nombre_Tipo { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }

        public int Telefono { get; set; }

        public string Nombre_Contacto { get; set; }
        public int Numero_Contacto { get; set; }

        public DateTime? Fecha_Insercion { get; set; }
        public string? Usuario_Insercion { get; set; }

        public DateTime? Fecha_Actualizacion { get; set; }
        public string? Usuario_Actualizacion { get; set; }

        public bool Estado { get; set; } = true;
    }

    public class ResponseDataProveedor
    {
        public List<Proveedor> DATA { get; set; }
    }

    

}
