using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;

namespace SmartStock.Controllers
{
    public class CompraController : Controller
    {
        private readonly APIService _apiService;
        private readonly IConfiguration _configuration;

        public CompraController(APIService apiService, IConfiguration configuration)
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
                ResponseDataProveedor? _proveedores =
                    JsonConvert.DeserializeObject<ResponseDataProveedor>(proveedores.ToString());

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

        [Authorize]
        public async Task<IActionResult> Ajustes()
        {
            return View("~/Views/Compras/Ajustes.cshtml");
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
    }
}
