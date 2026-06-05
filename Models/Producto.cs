

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace SmartStock.Models
{
    public class Producto
    {
        public int? ID_Producto { get; set; }

        // Unidad de Medida
        [Display(Name = "Unidad de Medida")]
        public int? ID_UnidadMedida { get; set; }
        public string? Nombre_UnidadMedida { get; set; }

        // Información del Producto
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Descripcion_Corta { get; set; }

        public decimal Precio { get; set; }

        // Impuesto
        [Display(Name = "Impuesto")]
        public int? ID_Impuesto { get; set; }
        public string? Nombre_Impuesto { get; set; }
        public decimal? Porcentaje { get; set; }

        // Proveedor
        [Display(Name = "Proveedor")]
        public int? ID_Proveedor { get; set; }
        public string? Nombre_Proveedor { get; set; }

        // Categoría
        [Display(Name = "Categoría")]
        public int? ID_Categoria { get; set; }
        public string? Categoria { get; set; }

        // Fabricante
        [Display(Name = "Fabricante")]
        public int? ID_Fabricante { get; set; }
        public string? Nombre_Fabricante { get; set; }

        // Inventario
        public string? Cod_Barras { get; set; }

        // Auditoría
        public bool Estado { get; set; } = true;

        // Usuario actual para guardar
        public string? Usuario { get; set; }
    }

    public class ResponseDataProducto
    {
        public List<Producto> DATA { get; set; }
    }

}
