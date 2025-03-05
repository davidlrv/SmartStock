
using System.Security.Cryptography;
using SmartStock.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartStock.Service
{
    public class AutenticacionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AutenticacionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> ObtenerToken()
        {
            try
            {
                var url = _configuration["settings:urlAPI-LGF"] + "Autenticacion/Validar";
                string correo = _configuration["settings:correoAPI-LGF"] ?? "";
                string clave = _configuration["settings:claveAPI-LGF"] ?? "";

                var usuario = new { correo, clave };
                var json = JsonSerializer.Serialize(usuario);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, data);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Error en la petición: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error en la petición: {ex.Message}");
            }


        }
      
    }
}
