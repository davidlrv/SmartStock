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

    }
}