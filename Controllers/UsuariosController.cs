using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;

namespace SmartStock.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly APIService _apiService;
        private readonly IConfiguration _configuration;

        public UsuariosController(APIService apiService, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiService = apiService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
                //LoginRequest loginRequest = new LoginRequest();
                //loginRequest.Usuario = usuario;
                //loginRequest.Clave = clave;
                loginRequest.Patron = _configuration["ConfigLogin:Patron"];

                var user = await _apiService.Run("sp_login", loginRequest);
                if (user.Contains("Data is Null"))
                {

                    TempData["Message"] = "Las credenciales ingresadas son incorrectas o el usuario no está registrado en nuestro sistema.";
                    TempData["MessageType"] = "success";

                    return View();
                }

                ResponseDataUser? _usuario = JsonConvert.DeserializeObject<ResponseDataUser>(user.ToString());

                if (_usuario != null)
                {

                    //2.- CONFIGURACION DE LA AUTENTICACION
                    #region AUTENTICACTION
                    var claims = new List<Claim>
                                {   new Claim("Id_Usuario", _usuario.DATA[0].Id_Usuario.ToString()),
                                    new Claim(ClaimTypes.Name, _usuario.DATA[0].Nombre),
                                    new Claim("Correo", _usuario.DATA[0].Correo),
                                };
                    foreach (Usuarios u in _usuario.DATA)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.Nombre_Rol));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    #endregion

                    //si se aacede con clave temporal, se manda directo al perfil
                    if (_usuario.DATA[0].Clave.Contains("T3mP"))
                        return RedirectToAction("Perfil", "Usuarios", new { Id_Usuario = _usuario.DATA[0].Id_Usuario });
                    else return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            

        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Super Administrador, Administrador")]
        public async Task<IActionResult> Usuarios()
        {
            var parametro = "{\"Activo\": \"1\"}";
            var user = await _apiService.Run("sp_Mostrar_Usuarios", parametro);

            if (user.Contains("Data is Null"))
            {
                return View();
            }
            else
            {
                ResponseDataUser? _usuario = JsonConvert.DeserializeObject<ResponseDataUser>(user.ToString());
                return View(_usuario.DATA);
            }

        }

    }
}
