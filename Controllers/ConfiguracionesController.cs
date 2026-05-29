using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;

namespace SmartStock.Controllers
{
    public class ConfiguracionesController : Controller
    {
        private readonly APIService _apiService;
        private readonly IConfiguration _configuration;

        public ConfiguracionesController(APIService apiService, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiService = apiService;
        }

        [Authorize]
        public async Task<IActionResult> ListaProveedor()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var proveedor = await _apiService.Run("sp_Mostrar_Proveedores", parametro);

            if (proveedor.Contains("Data is Null"))
            {
                return View("~/Views/Compras/Proveedores/ListaProveedor.cshtml");
            }
            else
            {
                ResponseDataProveedor? _proveedor = JsonConvert.DeserializeObject<ResponseDataProveedor>(proveedor.ToString());
                return View("~/Views/Compras/Proveedores/ListaProveedor.cshtml", _proveedor?.DATA);
            }
        }

        [Authorize]
        public async Task<IActionResult> Proveedor(Proveedor model)
        {
            if (model.ID_Proveedores == null)
            {
                model.Estado = true;
                return View("~/Views/Compras/Proveedores/Proveedor.cshtml", model);
            }

            var proveedores = await _apiService.Run("sp_Mostrar_Proveedores", model);

            if (proveedores.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Compras/Proveedores/Proveedor.cshtml", model);
            }
            else
            {
                ResponseDataProveedor? _proveedores = JsonConvert.DeserializeObject<ResponseDataProveedor>(proveedores.ToString());

                model = _proveedores.DATA[0];

                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Compras/Proveedores/Proveedor.cshtml", model);
                }
                else
                {
                    return View("~/Views/Compras/Proveedores/Proveedor.cshtml", model);
                }
            }
        }


        [Authorize]
        public async Task<IActionResult> GuardarProveedor(Proveedor model)
        {
            var proveedor = await _apiService.Run("sp_Guardar_Proveedores", model);

            if (proveedor.Contains("Data is Null") || proveedor.Contains("ErrorMessage"))
            {
                TempData["Message"] = "Hubo un error al guardar el proveedor. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Proveedor", new { ID_Proveedores = model.ID_Proveedores });
            }
            else
            {
                TempData["Message"] = "El proveedor se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataProveedor? _proveedor = JsonConvert.DeserializeObject<ResponseDataProveedor>(proveedor.ToString());

                return RedirectToAction("Proveedor", new { ID_Proveedores = _proveedor?.DATA[0].ID_Proveedores });
            }
        }

       
        [HttpGet]
        public async Task<IActionResult> GetTipos()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var tipo = await _apiService.Run("sp_Mostrar_Tipos", parametro);

            if (string.IsNullOrEmpty(tipo) || tipo.Contains("Data is Null"))
            {
                return Ok(new List<object>()); // Devuelve lista vacía si hay error
            }

            try
            {
                ResponseDataTipo? _Tipos = JsonConvert.DeserializeObject<ResponseDataTipo>(tipo);
                return Ok(_Tipos?.DATA);
            }
            catch (JsonException)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }


        [Authorize]
        public async Task<IActionResult> Opciones()
        {
            return View("~/Views/Configuraciones/Opciones.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> ListaImpuestos()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var Impuestos = await _apiService.Run("sp_Mostrar_Impuestos", parametro);

            if (Impuestos.Contains("Data is Null"))
            {
                return View("~/Views/Configuraciones/ListaImpuestos.cshtml");
            }
            else
            {
                ResponseDataImpuestos? _impuestos = JsonConvert.DeserializeObject<ResponseDataImpuestos>(Impuestos.ToString());
                return View("~/Views/Configuraciones/ListaImpuestos.cshtml", _impuestos ?.DATA);
            }
        }

        [Authorize]
        public async Task<IActionResult> Impuestos(Impuestos model)
        {
            if (model.ID_Impuesto == null)
            {
                model.Estado = true;
                return View("~/Views/Configuraciones/Impuesto.cshtml", model);
            }

            var impuestos = await _apiService.Run("sp_Mostrar_Impuestos", model);

            if (impuestos.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Configuraciones/Impuestos.cshtml", model);
            }
            else
            {
                ResponseDataImpuestos? _impuestos = JsonConvert.DeserializeObject<ResponseDataImpuestos>(impuestos.ToString());

                model = _impuestos.DATA[0];

                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Configuraciones/Impuestos.cshtml", model);
                }
                else
                {
                    return View("~/Views/Configuraciones/Impuestos.cshtml", model);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarImpuestos(Impuestos model)
        {
            var impuestos = await _apiService.Run("sp_Guardar_Impuestos", model);

            if (impuestos.Contains("Data is Null") || impuestos.Contains("Error"))
            {
                TempData["Message"] = "Hubo un error al guardar el impuesto. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Impuestos", new { ID_Impuesto = model.ID_Impuesto });
            }
            else
            {
                TempData["Message"] = "El impuesto se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataImpuestos? _impuesto = JsonConvert.DeserializeObject<ResponseDataImpuestos>(impuestos.ToString());

                return RedirectToAction("Impuestos", new { ID_Impuesto = _impuesto?.DATA[0].ID_Impuesto });
            }
        }

        [Authorize]
        public async Task<IActionResult> ListaFabricante()
        {
            var parametro = "{\"Estado\": \"1\"}";

            var fabricantes = await _apiService.Run("sp_Mostrar_Fabricante", parametro);

            if (fabricantes.Contains("Data is Null"))
            {
                return View("~/Views/Inventario/Fabricante/ListaFabricante.cshtml");
            }
            else
            {
                ResponseDataFabricante? _fabricantes =
                    JsonConvert.DeserializeObject<ResponseDataFabricante>(fabricantes.ToString());

                return View("~/Views/Inventario/Fabricante/ListaFabricante.cshtml",
                    _fabricantes?.DATA);
            }
        }

        [Authorize]
        public async Task<IActionResult> Fabricante(Fabricante model)
        {
            if (model.ID_Fabricante == null)
            {
                model.Estado = true;

                return View("~/Views/Inventario/Fabricante/Fabricante.cshtml", model);
            }

            var fabricante = await _apiService.Run("sp_Mostrar_Fabricante", model);

            if (fabricante.Contains("Data is Null"))
            {
                model.Estado = true;

                return View("~/Views/Inventario/Fabricante/Fabricante.cshtml", model);
            }
            else
            {
                ResponseDataFabricante? _fabricante =
                    JsonConvert.DeserializeObject<ResponseDataFabricante>(fabricante.ToString());

                model = _fabricante.DATA[0];

                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Inventario/Fabricante/Fabricante.cshtml", model);
                }
                else
                {
                    return View("~/Views/Inventario/Fabricante/Fabricante.cshtml", model);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarFabricante(Fabricante model)
        {
            var fabricante = await _apiService.Run("sp_Guardar_Fabricante", model);

            if (fabricante.Contains("Data is Null") || fabricante.Contains("Error"))
            {
                TempData["Message"] = "Hubo un error al guardar el fabricante. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Fabricante",
                    new { ID_Fabricante = model.ID_Fabricante });
            }
            else
            {
                TempData["Message"] = "El fabricante se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataFabricante? _fabricante =
                    JsonConvert.DeserializeObject<ResponseDataFabricante>(fabricante.ToString());

                return RedirectToAction("Fabricante",
                    new { ID_Fabricante = _fabricante?.DATA[0].ID_Fabricante });
            }
        }



    }
}
