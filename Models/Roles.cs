
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SmartStock.Models
{
    public class Roles
    {
        public int? Id_Rol { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre_Rol { get; set; }

    }

    public class ResponseDataRol
    {
        public List<Roles> DATA { get; set; }
    }

    

}
