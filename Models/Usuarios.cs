
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SmartStock.Models
{
    public class Usuarios
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo inválido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public string Nombre_Rol { get; set; }

        public bool Activo { get; set; }

        // Este campo se usa como hidden, no es necesario validarlo
        public int? Id_Usuario { get; set; }

        public string? Patron { get; set; }
        public string? UsuarioGuarda { get; set; }

        public string? recaptchaResponse { get; set; }

    }

    public class ResponseDataUser
    {
        public List<Usuarios> DATA { get; set; }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Patron { get; set; } // Puede ser usado para una clave o patrón de encriptación
    }

}
