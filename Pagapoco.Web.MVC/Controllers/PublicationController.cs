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

        // Listado general de publicaciones (público)
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:5001/api/publications");
            if (!response.IsSuccessStatusCode)
                return View(new List<PublicationViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var publications = JsonSerializer.Deserialize<List<PublicationViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(publications ?? new List<PublicationViewModel>());
        }

        // Detalle de una publicación (público)
        public async Task<IActionResult> Details(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:5001/api/publications/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var publication = JsonSerializer.Deserialize<PublicationViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(publication);
        }

        // Muestra las publicaciones del usuario logueado
        public async Task<IActionResult> MyPublications()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "User");

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:5001/api/publications/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return View(new List<UserPublicationViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var publications = JsonSerializer.Deserialize<List<PublicationViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var result = new List<UserPublicationViewModel>();
            foreach (var pub in publications ?? new List<PublicationViewModel>())
            {
                string? mainImageUrl = null;
                var imgResponse = await client.GetAsync($"https://localhost:5001/api/images/publication/{pub.Title}"); // Cambia pub.Title por pub.Id si lo necesitas internamente
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

        // Filtro de publicaciones (público)
        public async Task<IActionResult> Filter(string? type, string? brand, string? model, string? color, string? condition, string? compatibility)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://localhost:5001/api/publications/filter?type={type}&brand={brand}&model={model}&color={color}&condition={condition}&compatibility={compatibility}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View("Index", new List<PublicationViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var publications = JsonSerializer.Deserialize<List<PublicationViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View("Index", publications ?? new List<PublicationViewModel>());
        }
    }
}