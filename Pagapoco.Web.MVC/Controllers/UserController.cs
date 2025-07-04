using Microsoft.AspNetCore.Mvc;
using Pagapoco.Web.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                // Aquí puedes guardar la sesión/cookie según tu lógica
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
                // Aquí puedes guardar la sesión/cookie según tu lógica
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "No se pudo registrar el usuario");
            return View(model);
        }

        public IActionResult Logout()
        {
            // Limpia la sesión/cookie según tu lógica
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            // Aquí deberías obtener los datos del usuario actual (por ejemplo, desde la API)
            // y pasarlos a la vista
            return View(/* modelo de usuario */);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Llama a la API para actualizar el usuario
            // ...

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            // Llama a la API para eliminar el usuario actual
            // ...

            // Limpia la sesión/cookie si corresponde
            return RedirectToAction("Login");
        }
    }
}