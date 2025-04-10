using Microsoft.AspNetCore.Mvc;

namespace SmartStock.Controllers.Ventas.Caja
{
    public class CajaController : Controller
    {
        public IActionResult Caja()
        {
            return View("~/Views/Ventas/Caja/Caja.cshtml");
        }
    }
}
