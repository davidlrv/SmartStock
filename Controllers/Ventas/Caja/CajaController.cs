using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;

namespace SmartStock.Controllers.Ventas
{
    public class CajaController : Controller
    {
        private readonly APIService _apiService;
        private readonly IConfiguration _configuration;

        public CajaController(APIService apiService, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiService = apiService;
        }
        public async Task<IActionResult> ListaCaja()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var caja = await _apiService.Run("sp_Mostrar_Caja", parametro);

            if (caja.Contains("Data is Null"))
            {
                return View("~/Views/Ventas/Caja/ListaCaja.cshtml");
            }
            else
            {
                ResponseDataCaja? _caja = JsonConvert.DeserializeObject<ResponseDataCaja>(caja.ToString());
                return View("~/Views/Ventas/Caja/ListaCaja.cshtml", _caja?.DATA);
            }
        }
        public async Task<IActionResult> Caja(Caja model)
        {
            if (model.ID_Caja == null)
            {
                model.Estado = true;
                return View("~/Views/Ventas/Caja/Caja.cshtml", model); // Retorna la vista de caja sin datos
            }
            var caja = await _apiService.Run("sp_Mostrar_Caja", model);

            if (caja.Contains("Data is Null"))
            {
                model.Estado = true;
                return View("~/Views/Ventas/Caja/Caja.cshtml", model); // Retorna la vista de caja sin datos
            }
            else
            {
                ResponseDataCaja? _caja = JsonConvert.DeserializeObject<ResponseDataCaja>(caja.ToString());
                model = _caja.DATA[0];
                ModelState.ClearValidationState(nameof(model.Cod_Caja));
                ModelState.ClearValidationState(nameof(model.Estado));
                TryValidateModel(model);
                if (!ModelState.IsValid)
                {
                    // Manejar el estado no válido del modelo si es necesario
                    return View("~/Views/Ventas/Caja/Caja.cshtml", model);
                }
                else
                    return View("~/Views/Ventas/Caja/Caja.cshtml", model); // Retorna la vista Perfil con los datos del usuario
            }
        }
       
    }
}
