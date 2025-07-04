using Microsoft.AspNetCore.Mvc;
using Pagapoco.Web.MVC.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pagapoco.Web.MVC.Controllers
{
    public class PublicationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Muestra las publicaciones del usuario logueado
        public async Task<IActionResult> MyPublications()
        {
            // Obtén el userId del usuario logueado (ajusta según tu lógica de autenticación)
            var userId = HttpContext.Session.GetString("UserId"); // Ejemplo: Obtener el userId desde la sesión
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "User");

            var client = _httpClientFactory.CreateClient();

            // 1. Obtener publicaciones del usuario
            var response = await client.GetAsync($"https://localhost:5001/api/publications/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return View(new List<UserPublicationViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var publications = JsonSerializer.Deserialize<List<PublicationViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var result = new List<UserPublicationViewModel>();

            // 2. Para cada publicación, obtener la imagen principal
            foreach (var pub in publications)
            {
                string? mainImageUrl = null;
                var imgResponse = await client.GetAsync($"https://localhost:5001/api/images/publication/{pub.Id}");
                if (imgResponse.IsSuccessStatusCode)
                {
                    var imgJson = await imgResponse.Content.ReadAsStringAsync();
                    var images = JsonSerializer.Deserialize<List<ImageViewModel>>(imgJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    mainImageUrl = images?.Count > 0 ? images[0].Url : null;
                }

                result.Add(new UserPublicationViewModel
                {
                    Publication = pub,
                    MainImageUrl = mainImageUrl
                });
            }

            return View(result);
        }
    }
}