using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class CategoriaProductos
    {
        public int? ID_Categoria { get; set; }

        public string Categoria { get; set; }

        public bool Estado { get; set; } = true;

        public string? Usuario { get; set; }
    }

    public class ResponseDataCategoriaProductos
    {
        public List<CategoriaProductos> DATA { get; set; }
    }
}