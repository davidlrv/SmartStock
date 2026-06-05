using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;

namespace SmartStock.Controllers
{
    public class InventarioController : Controller
    {
        private readonly APIService _apiService;
        private readonly IConfiguration _configuration;

        public InventarioController(APIService apiService, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiService = apiService;
        }
        [Authorize]
        public async Task<IActionResult> ListaBodega()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var bodega = await _apiService.Run("sp_Mostrar_Bodega", parametro);

            if (bodega.Contains("Data is Null"))
            {
                return View("~/Views/Inventario/Bodega/ListaBodega.cshtml");
            }
            else
            {
                ResponseDataBodega? _Bodega = JsonConvert.DeserializeObject<ResponseDataBodega>(bodega.ToString());
                return View("~/Views/Inventario/Bodega/ListaBodega.cshtml", _Bodega?.DATA);
            }
        }
        [Authorize]
        public async Task<IActionResult> Bodega(Bodega model)
        {
            if (model.ID_Bodega == null)
            {
                model.Estado = true;
                return View("~/Views/Inventario/Bodega/Bodega.cshtml", model); // Retorna la vista de bodega sin datos
            }
            var bodega = await _apiService.Run("sp_Mostrar_Bodega", model);

            if (bodega.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Inventario/Bodega/Bodega.cshtml",model); // Retorna la vista de bodega sin datos
            }
            else
            {
                ResponseDataBodega? _bodega = JsonConvert.DeserializeObject<ResponseDataBodega>(bodega.ToString());
                model = _bodega.DATA[0];
                ModelState.ClearValidationState(nameof(model.Nombre));
                ModelState.ClearValidationState(nameof(model.Estado));
                TryValidateModel(model);
                if (!ModelState.IsValid)
                {
                    // Manejar el estado no válido del modelo si es necesario
                    return View("~/Views/Inventario/Bodega/Bodega.cshtml",model);
                }
                else
                    return View("~/Views/Inventario/Bodega/Bodega.cshtml",model); // Retorna la vista Perfil con los datos del usuario
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarBodega(Bodega model)
        {

            var bodega = await _apiService.Run("sp_Guardar_Bodega", model);

            if (bodega.Contains("Data is Null") || bodega.Contains("ErrorMessage"))
            {
                // Si hay un error, podrías enviar un mensaje de error a la vista, si lo deseas.
                TempData["Message"] = "Hubo un error al guardar la bodega. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "success"; // Puedes utilizar esto para determinar el tipo de mensaje en la vista.
                return RedirectToAction("Bodega", new { ID_Bodega = model.ID_Bodega });
            }
            else
            {
                // Guardado exitoso, pasamos un mensaje de éxito a la vista.
                TempData["Message"] = "La Bodega se guardo correctamente.";
                TempData["MessageType"] = "success"; // Esto es opcional, pero puede ser útil para definir el estilo del mensaje.
                ResponseDataBodega? _bodega = JsonConvert.DeserializeObject<ResponseDataBodega>(bodega.ToString());
                return RedirectToAction("Bodega", new { ID_Bodega = _bodega?.DATA[0].ID_Bodega });
            }

        }
        [Authorize]
        public async Task<IActionResult> ListaCategorias()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var categoria = await _apiService.Run("sp_Mostrar_Categoria_Productos", parametro);

            if (categoria.Contains("Data is Null"))
            {
                return View("~/Views/Inventario/Categorias/ListaCategorias.cshtml");
            }
            else
            {
                ResponseDataCategoriaProductos? _categoria = JsonConvert.DeserializeObject<ResponseDataCategoriaProductos>(categoria.ToString());
                return View("~/Views/Inventario/Categorias/ListaCategorias.cshtml", _categoria?.DATA);
            }
        }
       
        [Authorize]
        public async Task<IActionResult> Categorias(CategoriaProductos model)
        {
            if (model.ID_Categoria == null)
            {
                model.Estado = true;
                return View("~/Views/Inventario/Categorias/Categorias.cshtml", model); // Retorna la vista de bodega sin datos
            }
            var categorias = await _apiService.Run("sp_Mostrar_Categoria_Productos", model);

            if (categorias.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Inventario/Categorias/Categorias.cshtml", model); // Retorna la vista de bodega sin datos
            }
            else
            {
                ResponseDataCategoriaProductos? _categorias = JsonConvert.DeserializeObject<ResponseDataCategoriaProductos>(categorias.ToString());
                model = _categorias.DATA[0];
                //ModelState.ClearValidationState(nameof(model.Nombre));
                //ModelState.ClearValidationState(nameof(model.Estado));
                TryValidateModel(model);
                if (!ModelState.IsValid)
                {
                    // Manejar el estado no válido del modelo si es necesario
                    return View("~/Views/Inventario/Categorias/Categorias.cshtml", model);
                }
                else
                    return View("~/Views/Inventario/Categorias/Categorias.cshtml", model); // Retorna la vista Perfil con los datos del usuario
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarCategorias(CategoriaProductos model)
        {
            var categoria = await _apiService.Run("sp_Guardar_Categoria_Productos", model);

            if (categoria.Contains("Data is Null") || categoria.Contains("ErrorMessage"))
            {
                TempData["Message"] = "Hubo un error al guardar la categoría. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Categorias", new { ID_Categoria = model.ID_Categoria });
            }
            else
            {
                TempData["Message"] = "La categoría se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataCategoriaProductos? _categoria = JsonConvert.DeserializeObject<ResponseDataCategoriaProductos>(categoria.ToString());

                return RedirectToAction("Categorias", new { ID_Categoria = _categoria?.DATA[0].ID_Categoria });
            }
        }


        [Authorize]
        public async Task<IActionResult> Ajustes()
        {
            return View("~/Views/Inventario/Ajustes.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> ListaUnidadMedida()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var UnidadMedida = await _apiService.Run("sp_Mostrar_UnidadMedida", parametro);

            if (UnidadMedida.Contains("Data is Null"))
            {
                return View("~/Views/Inventario/UnidadMedida/ListaUnidadMedida.cshtml");
            }
            else
            {
                ResponseDataUnidadMedida? _UnidadMedida = JsonConvert.DeserializeObject<ResponseDataUnidadMedida>(UnidadMedida.ToString());
                return View("~/Views/Inventario/UnidadMedida/ListaUnidadMedida.cshtml", _UnidadMedida?.DATA);
            }
        }

        [Authorize]
        public async Task<IActionResult> UnidadMedida(UnidadMedida model)
        {
            if (model.ID_UnidadMedida == null)
            {
                model.Estado = true;

                return View("~/Views/Inventario/UnidadMedida/UnidadMedida.cshtml", model);
            }

            var unidadMedida = await _apiService.Run("sp_Mostrar_UnidadMedida", model);

            if (unidadMedida.Contains("Data is Null"))
            {
                model.Estado = true;

                return View("~/Views/Inventario/UnidadMedida/UnidadMedida.cshtml", model);
            }
            else
            {
                ResponseDataUnidadMedida? _unidadMedida =
                    JsonConvert.DeserializeObject<ResponseDataUnidadMedida>(unidadMedida.ToString());

                model = _unidadMedida.DATA[0];

                //ModelState.ClearValidationState(nameof(model.Nombre_UnidadMedida));
                //ModelState.ClearValidationState(nameof(model.Estado));

                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Inventario/UnidadMedida/UnidadMedida.cshtml", model);
                }
                else
                {
                    return View("~/Views/Inventario/UnidadMedida/UnidadMedida.cshtml", model);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarUnidadMedida(UnidadMedida model)
        {
            var unidadMedida = await _apiService.Run("sp_Guardar_UnidadMedida", model);

            if (unidadMedida.Contains("Data is Null") || unidadMedida.Contains("ErrorMessage"))
            {
                TempData["Message"] = "Hubo un error al guardar la unidad de medida. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("UnidadMedida", new { ID_UnidadMedida = model.ID_UnidadMedida });
            }
            else
            {
                TempData["Message"] = "La unidad de medida se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataUnidadMedida? _unidadMedida =
                    JsonConvert.DeserializeObject<ResponseDataUnidadMedida>(unidadMedida.ToString());

                return RedirectToAction("UnidadMedida",
                    new { ID_UnidadMedida = _unidadMedida?.DATA[0].ID_UnidadMedida });
            }
        }


        [Authorize]
        public async Task<IActionResult> ListaProductos()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var productos = await _apiService.Run("sp_Mostrar_Productos", parametro);

            if (productos.Contains("Data is Null"))
            {
                return View("~/Views/Inventario/Productos/ListaProductos.cshtml");
            }
            else
            {
                ResponseDataProducto? _productos =
                    JsonConvert.DeserializeObject<ResponseDataProducto>(productos.ToString());

                return View(
                    "~/Views/Inventario/Productos/ListaProductos.cshtml",
                    _productos?.DATA
                );
            }
        }

        [Authorize]
        public async Task<IActionResult> Producto(Producto model)
        {
            if (model.ID_Producto == null)
            {
                model.Estado = true;
                return View("~/Views/Inventario/Productos/Producto.cshtml", model);
            }

            var producto = await _apiService.Run("sp_Mostrar_Productos", model);

            if (producto.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Inventario/Productos/Producto.cshtml", model);
            }

            ResponseDataProducto? _producto =
                JsonConvert.DeserializeObject<ResponseDataProducto>(producto);

            model = _producto.DATA[0];

            TryValidateModel(model);

            return View("~/Views/Inventario/Productos/Producto.cshtml", model);
        }

        [Authorize]
        public async Task<IActionResult> GuardarProducto(Producto model)
        {
            var producto = await _apiService.Run("sp_Guardar_Productos", model);

            if (producto.Contains("Data is Null")
                || producto.Contains("Error")
                || producto.Contains("ErrorMessage"))
            {
                TempData["Message"] = "Hubo un error al guardar el producto. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Producto",
                    new { ID_Producto = model.ID_Producto });
            }

            TempData["Message"] = "El producto se guardó correctamente.";
            TempData["MessageType"] = "success";

            ResponseDataProducto? _producto =
                JsonConvert.DeserializeObject<ResponseDataProducto>(producto);

            return RedirectToAction("Producto",
                new { ID_Producto = _producto?.DATA[0].ID_Producto });
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var categoria = await _apiService.Run("sp_Mostrar_Categoria_Productos", parametro);

            if (string.IsNullOrEmpty(categoria) || categoria.Contains("Data is Null"))
            {
                return Ok(new List<object>());
            }

            try
            {
                ResponseDataCategoriaProductos? _categorias =
                    JsonConvert.DeserializeObject<ResponseDataCategoriaProductos>(categoria);

                return Ok(_categorias?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProveedores()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var proveedor = await _apiService.Run("sp_Mostrar_Proveedores", parametro);

            if (string.IsNullOrEmpty(proveedor) || proveedor.Contains("Data is Null"))
            {
                return Ok(new List<object>());
            }

            try
            {
                ResponseDataProveedor? _proveedores =
                    JsonConvert.DeserializeObject<ResponseDataProveedor>(proveedor);

                return Ok(_proveedores?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFabricantes()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var fabricante = await _apiService.Run("sp_Mostrar_Fabricante", parametro);

            if (string.IsNullOrEmpty(fabricante) || fabricante.Contains("Data is Null"))
            {
                return Ok(new List<object>());
            }

            try
            {
                ResponseDataFabricante? _fabricantes =
                    JsonConvert.DeserializeObject<ResponseDataFabricante>(fabricante);

                return Ok(_fabricantes?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnidadMedida()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var unidad = await _apiService.Run("sp_Mostrar_UnidadMedida", parametro);

            if (string.IsNullOrEmpty(unidad) || unidad.Contains("Data is Null"))
            {
                return Ok(new List<object>());
            }

            try
            {
                ResponseDataUnidadMedida? _unidades =
                    JsonConvert.DeserializeObject<ResponseDataUnidadMedida>(unidad);

                return Ok(_unidades?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImpuestos()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var impuesto = await _apiService.Run("sp_Mostrar_Impuestos", parametro);

            if (string.IsNullOrEmpty(impuesto) || impuesto.Contains("Data is Null"))
            {
                return Ok(new List<object>());
            }

            try
            {
                ResponseDataImpuestos? _impuestos =
                    JsonConvert.DeserializeObject<ResponseDataImpuestos>(impuesto);

                return Ok(_impuestos?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }
    }
}