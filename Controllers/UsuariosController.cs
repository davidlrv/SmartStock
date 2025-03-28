using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartStock.Models;
using SmartStock.Service;
using System.Data;

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

        [Authorize]
        public async Task<IActionResult> Perfil(Usuarios model)
        {
            model.Patron = _configuration["ConfigLogin:Patron"];
            var user = await _apiService.Run("sp_Mostrar_Usuarios", model);

            if (user.Contains("Data is Null"))
            {
                model.Activo = true;
                return View(model); // Retorna la vista Perfil sin datos
            }
            else
            {
                ResponseDataUser? _usuario = JsonConvert.DeserializeObject<ResponseDataUser>(user.ToString());
                model = _usuario.DATA[0];
                model.Patron = _configuration["ConfigLogin:Patron"];
                ModelState.ClearValidationState(nameof(model.Usuario));
                ModelState.ClearValidationState(nameof(model.Nombre));
                ModelState.ClearValidationState(nameof(model.Correo));
                ModelState.ClearValidationState(nameof(model.Activo));
                ModelState.ClearValidationState(nameof(model.Clave));
                ModelState.ClearValidationState(nameof(model.Id_Usuario));
                ModelState.ClearValidationState(nameof(model.Nombre_Rol));
                ModelState.ClearValidationState(nameof(model.Patron));
                TryValidateModel(model);
                if (!ModelState.IsValid)
                {
                    // Manejar el estado no válido del modelo si es necesario
                    return View(model);
                }
                else
                    return View(model); // Retorna la vista Perfil con los datos del usuario
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarPerfil(Usuarios model)
        {
                model.Patron = _configuration["ConfigLogin:Patron"];

                var user = await _apiService.Run("sp_Guardar_Usuario", model);

                if (user.Contains("Data is Null") || user.Contains("ErrorMessage"))
                {
                    // Si hay un error, podrías enviar un mensaje de error a la vista, si lo deseas.
                    TempData["Message"] = "Hubo un error al guardar el perfil. Por favor, intenta de nuevo.";
                    TempData["MessageType"] = "success"; // Puedes utilizar esto para determinar el tipo de mensaje en la vista.
                    return RedirectToAction("Perfil", new { Id_Usuario = model.Id_Usuario });
                }
                else
                {
                    // Guardado exitoso, pasamos un mensaje de éxito a la vista.
                    TempData["Message"] = "El perfil se guardo correctamente.";
                    TempData["MessageType"] = "success"; // Esto es opcional, pero puede ser útil para definir el estilo del mensaje.
                    ResponseDataUser? _usuario = JsonConvert.DeserializeObject<ResponseDataUser>(user.ToString());
                    return RedirectToAction("Perfil", new { Id_Usuario = _usuario.DATA[0].Id_Usuario });
                }
        
        }

        public IActionResult RecuperarPass()
        {
            return View();
        }
        public async Task<IActionResult> Recuperar(String Correo, string recaptchaResponse)
        {
            dynamic parametros = new
                {
                    Correo = Correo,
                    Patron = _configuration["ConfigLogin:Patron"]
                };


                var user = await _apiService.Run("sp_Recuperar_Login", parametros);
                if (user.Contains("Data is Null"))
                {

                    TempData["Message"] = "Usuario no encontrador.";
                    TempData["MessageType"] = "success";

                    return View();
                }

                ResponseDataUser? _usuario = JsonConvert.DeserializeObject<ResponseDataUser>(user.ToString());

                if (_usuario != null)
                {

                    TempData["MessageType"] = "success";

                    //Enviar Correo con la clave de recuperacion
                    var estado = await SendEmailPass(_usuario.DATA[0]);

                    if (!string.IsNullOrEmpty(estado.ToString()))
                    {
                        TempData["Message"] = "Se enviaron credenciales temporales al correo indicado";
                    }
                    else
                    {
                        TempData["Message"] = "Ocurrió un error al intentar enviar el correo:";
                    }
                    // Redirige a la acción RecuperarPass
                    return RedirectToAction("RecuperarPass");
                }
                else
                {
                    TempData["MessageType"] = "success";
                    TempData["Message"] = "Usuario no registrado!";
                    return RedirectToAction("RecuperarPass");
                }
            
     }

        public async Task<IActionResult> SendEmailPass(Usuarios User)
        {
            try
            {
                EmailModel email = new EmailModel();

                email.Content = ""
                                 + "<body style=\"font-family:Arial,sans-serif;color:#333;margin:0;padding:0;background-color:#fff;\">"
                                 + "<div style=\"width:100%;max-width:600px;margin:0 auto;background-color:#FFF5EC;padding:20px;box-shadow:0 0 10px rgba(0,0,0,0.1);\">"
                                 + "<div align=\"center\" class=\"img-container center\" style=\"padding-right:0px;padding-left:0px;\">"
                                 + "<img class=\"center\" style=\"margin:0 auto 0 0;text-align:center !important;border:0px none;height:auto;max-width:375px;\" title=\"LaGranFrancia\" src=\"https://www.lagranfrancia.com/Assets/images/logoLGF-black.png\" alt=\"LaGranFrancia\" width=\"375\" border=\"0\" align=\"middle\" loading=\"lazy\">"
                                 + "<div style=\"line-height:20px;font-size:1px;\">&nbsp;</div></div>"
                                 + "<div style=\"background-color:#BF8C2C;color:#fff;padding:10px 20px;text-align:center;\">"
                                 + "<h1 style=\"margin:0;\">Recuperación de Credenciales</h1></div>"
                                 + "<div style=\"padding:20px;\">"
                                 + "<p style=\"font-size:16px;line-height:1.6;\">Estimado/a [Nombre_deldestinatario],</p>"
                                 + "<p style=\"font-size:16px;line-height:1.6;\">Se ha restablecido exitosamente su contraseña de acceso. Por motivos de seguridad, se le ha asignado una contraseña temporal. Le recomendamos encarecidamente que cambie esta contraseña en su próximo inicio de sesión.</p>"
                                 + "<p style=\"font-size:16px;line-height:1.6;\"><b>Usuario: [username]</b></p>"
                                 + "<p style=\"font-size:16px;line-height:1.6;\"><b>Clave Temporal: [pass]</b></p>"
                                 + "<p style=\"font-size:16px;line-height:1.6;\">¡No olvides crear una contraseña súper fuerte! Tu seguridad en línea depende de ello.</p>"
                                 + "<p style=\"font-size:16px;line-height:1.6;\">Saludos cordiales,<br>Administración</p></div>"
                                 + "<div style=\"margin-top:20px;text-align:center;font-size:12px;color:#777;\">"
                                 + "<p>© [yyyy] Hotel La Gran Francia. Todos los derechos reservados.</p>"
                                 + "<p style=\"text-align:center;\">"
                                 + "<a href=\"https://www.lagranfrancia.com/usuarios/login\" style=\"display:inline-block;padding:10px 20px;background-color:#BF8C2C;color:#fff;text-decoration:none;font-size:16px;border-radius:5px;\">"
                                 + "www.lagranfrancia.com</a></p></div></div></body>";




                email.Content = email.Content.Replace("[Nombre_deldestinatario]", User.Nombre);
                email.Content = email.Content.Replace("[username]", User.Usuario);
                email.Content = email.Content.Replace("[pass]", User.Clave);
                email.Content = email.Content.Replace("[yyyy]", DateTime.Now.Year.ToString());

                email.Subject = "Recuperación de Credenciales";
                email.Recipient = User.Correo;
                email.RecipientName = User.Nombre;

                var Repuesta = await _apiService.SendEmail(email);

                return Json(new { Repuesta = Repuesta.ToString() });
            }
            catch (HttpRequestException ex)
            {
                //return StatusCode(500, ex.Message);
                return Json(new { success = false });
            }
        }

       // [Authorize(Roles = "Super Administrador, Administrador")]
        public async Task<IActionResult> ListaRoles()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var rol = await _apiService.Run("sp_Mostrar_Roles", parametro);

            if (rol.Contains("Data is Null"))
            {
                return View();
            }
            else
            {
                ResponseDataRol? _Roles = JsonConvert.DeserializeObject<ResponseDataRol>(rol.ToString());
                return View(_Roles?.DATA);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var parametro = "{\"Estado\": \"1\"}";
            var rol = await _apiService.Run("sp_Mostrar_Roles", parametro);

            if (string.IsNullOrEmpty(rol) || rol.Contains("Data is Null"))
            {
                return Ok(new List<object>()); // Devuelve una lista vacía en caso de error
            }

            try
            {
                ResponseDataRol? _Roles = JsonConvert.DeserializeObject<ResponseDataRol>(rol);
                return Ok(_Roles?.DATA);
            }
            catch (JsonException ex)
            {
                return BadRequest("Error al deserializar la respuesta de la API.");
            }
        }



        public async Task<IActionResult> Roles(Roles model)
        {
            if (model.Id_Rol == null) {
                model.Estado = true;
                return View(model); // Retorna la vista Perfil sin datos
            }
            var rol = await _apiService.Run("sp_Mostrar_Roles", model);

            if (rol.Contains("Data is Null"))
            {
                model.Estado = true;
                return View(model); // Retorna la vista Perfil sin datos
            }
            else
            {
                ResponseDataRol? _rol = JsonConvert.DeserializeObject<ResponseDataRol>(rol.ToString());
                model = _rol.DATA[0];
                ModelState.ClearValidationState(nameof(model.Nombre_Rol));
                ModelState.ClearValidationState(nameof(model.Estado));
                TryValidateModel(model);
                if (!ModelState.IsValid)
                {
                    // Manejar el estado no válido del modelo si es necesario
                    return View(model);
                }
                else
                    return View(model); // Retorna la vista Perfil con los datos del usuario
            }
        }

        [Authorize]
        public async Task<IActionResult> GuardarRoles(Roles model)
        {
           
            var rol = await _apiService.Run("sp_Guardar_Rol", model);

            if (rol.Contains("Data is Null") || rol.Contains("ErrorMessage"))
            {
                // Si hay un error, podrías enviar un mensaje de error a la vista, si lo deseas.
                TempData["Message"] = "Hubo un error al guardar el Rol. Por favor, intenta de nuevo.";
                TempData["MessageType"] = "success"; // Puedes utilizar esto para determinar el tipo de mensaje en la vista.
                return RedirectToAction("Roles", new { Id_Rol = model.Id_Rol });
            }
            else
            {
                // Guardado exitoso, pasamos un mensaje de éxito a la vista.
                TempData["Message"] = "El Rol se guardo correctamente.";
                TempData["MessageType"] = "success"; // Esto es opcional, pero puede ser útil para definir el estilo del mensaje.
                ResponseDataRol? _rol = JsonConvert.DeserializeObject<ResponseDataRol>(rol.ToString());
                return RedirectToAction("Roles", new { Id_Rol = _rol?.DATA[0].Id_Rol });
            }

        }


    }
}
