using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using SmartStock.Models;


namespace SmartStock.Service
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AutenticacionService _autenticacionService;
        private readonly IHttpClientFactory _httpClientFactory;
    

        public APIService(HttpClient httpClient, IConfiguration configuration, AutenticacionService autenticacionService, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _autenticacionService = autenticacionService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Run(String sp, dynamic parametro)
        {
            try
            {
                var url = _configuration["ConfigAPI:urlAPI"] + "VersaLinks/" + sp;
              
                var json = System.Text.Json.JsonSerializer.Serialize(parametro);
                var data = new StringContent(json, Encoding.UTF8, "application/json");


                var r = await _autenticacionService.ObtenerToken();
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(r.ToString());


                // Añadir el token al encabezado de autorización
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.Token);


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

        public async Task<string> SendEmail(EmailModel email)
        {
            var url = _configuration["ConfigAPI:urlAPI"] + "VersaLinks/SendEmail";

            try
            {
                var emailJson = JsonConvert.SerializeObject(email);
                var content = new StringContent(emailJson, Encoding.UTF8, "application/json");


                var r = await _autenticacionService.ObtenerToken();
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(r.ToString());


                // Agregar el token de autenticación en el encabezado
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse?.Token);

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Manejar la respuesta de error (puedes registrar o analizar la respuesta)
                    throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error en la petición: {ex.Message}");
            }
        
        }

    }
}
