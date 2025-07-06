using Microsoft.AspNetCore.Mvc;
using Pagapoco.Web.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;

namespace Pagapoco.Web.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:5001/api/user/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseBody);
                var token = jsonDoc.RootElement.GetProperty("token").GetString();

                // Guardar el JWT en una cookie segura
                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(2)
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:5001/api/user/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Opcional: podrías hacer login automático aquí
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "No se pudo registrar el usuario");
            return View(model);
        }

        public IActionResult Logout()
        {
            // Eliminar la cookie del JWT
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            // Decodificar el userId del JWT
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Obtener datos del usuario autenticado
            var response = await client.GetAsync($"https://localhost:5001/api/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            var userJson = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserEditViewModel>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Solo enviamos los campos editables
            var updateModel = new
            {
                Name = model.Name,
                Phone = model.Phone,
                City = model.City
            };
            var json = JsonSerializer.Serialize(updateModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:5001/api/user/{userId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "No se pudo actualizar el usuario");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"https://localhost:5001/api/user/{userId}");

            // Eliminar la cookie del JWT
            Response.Cookies.Delete("jwt_token");

            return RedirectToAction("Login");
        }
    }
}