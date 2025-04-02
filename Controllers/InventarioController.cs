using Microsoft.AspNetCore.Mvc;

namespace SmartStock.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult ListaBodega()
        {
            return View("~/Views/Inventario/Bodega/ListaBodega.cshtml");

        }
    }
}
