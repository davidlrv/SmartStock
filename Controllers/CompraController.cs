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

        [Authorize]
        public async Task<IActionResult> ListaTipos()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var tipos = await _apiService.Run("sp_Mostrar_Tipos", parametro);

            if (tipos.Contains("Data is Null"))
            {
                return View("~/Views/Compras/TipoProveedor/ListaTipos.cshtml");
            }
            else
            {
                ResponseDataTipo? _tipos = JsonConvert.DeserializeObject<ResponseDataTipo>(tipos.ToString());
                return View("~/Views/Compras/TipoProveedor/ListaTipos.cshtml", _tipos?.DATA);
            }
        }

        [Authorize]
        public async Task<IActionResult> Tipo(Tipo model)
        {
            if (model.ID_Tipo == null)
            {
                model.Estado = true;
                return View("~/Views/Compras/TipoProveedor/Tipo.cshtml", model);
            }

            var tipos = await _apiService.Run("sp_Mostrar_Tipos", model);

            if (tipos.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Compras/TipoProveedor/Tipo.cshtml", model);
            }
            else
            {
                ResponseDataTipo? _tipos = JsonConvert.DeserializeObject<ResponseDataTipo>(tipos.ToString());

                model = _tipos.DATA[0];

                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Compras/TipoProveedor/Tipo.cshtml", model);
                }
                else
                {
                    return View("~/Views/Compras/TipoProveedor/Tipo.cshtml", model);
                }
            }
        }
        

        [Authorize]
        public async Task<IActionResult> GuardarTipo(Tipo model)
        {
            var tipo = await _apiService.Run("sp_Guardar_Tipo", model);

            if (tipo.Contains("Data is Null") || tipo.Contains("ErrorMessage"))
            {
                TempData["Message"] = "Hubo un error al guardar el Tipo. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "danger";

                return RedirectToAction("Tipo", new { ID_Tipo = model.ID_Tipo });
            }
            else
            {
                TempData["Message"] = "El Tipo se guardó correctamente.";
                TempData["MessageType"] = "success";

                ResponseDataTipo? _tipo = JsonConvert.DeserializeObject<ResponseDataTipo>(tipo.ToString());

                return RedirectToAction("Tipo", new { ID_Tipo = _tipo?.DATA[0].ID_Tipo });
            }
        }
    }
}
