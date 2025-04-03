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
            return View("~/Views/Inventario/Bodega/Bodega.cshtml");
        }
    }
}