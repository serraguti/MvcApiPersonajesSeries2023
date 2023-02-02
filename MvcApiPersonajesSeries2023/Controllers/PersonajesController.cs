using Microsoft.AspNetCore.Mvc;
using MvcApiPersonajesSeries2023.Models;
using MvcApiPersonajesSeries2023.Services;

namespace MvcApiPersonajesSeries2023.Controllers
{
    public class PersonajesController : Controller
    {
        private ServiceSeries service;

        public PersonajesController(ServiceSeries service)
        {
            this.service = service;
        }

        public async Task<IActionResult> PersonajesSerie(int idserie)
        {
            List<Personaje> personajes =
                await this.service.GetPersonajesSerieAsync(idserie);
            return View(personajes);
        }
    }
}
