using Microsoft.AspNetCore.Mvc;
using MvcApiPersonajesSeries2023.Helpers;
using MvcApiPersonajesSeries2023.Models;
using MvcApiPersonajesSeries2023.Services;

namespace MvcApiPersonajesSeries2023.Controllers
{
    public class PersonajesController : Controller
    {
        private ServiceSeries service;
        private HelperPathProvider helperPath;

        public PersonajesController(ServiceSeries service
            , HelperPathProvider helperPath)
        {
            this.service = service;
            this.helperPath = helperPath;
        }

        public async Task<IActionResult> PersonajesSerie(int idserie)
        {
            List<Personaje> personajes =
                await this.service.GetPersonajesSerieAsync(idserie);
            return View(personajes);
        }

        public async Task<IActionResult> CreatePersonaje()
        {
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["SERIES"] = series;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonaje
            (Personaje personaje, IFormFile fichero)
        {
            //DEBEMOS SUBIR EL FICHERO A NUESTRO SERVIDOR
            string fileName = fichero.FileName;
            string path = this.helperPath.GetMapPath(Folders.Imagenes, fileName);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            //POR ULTIMO, GUARDAMOS EN EL API LA URL DE NUESTRO SERVIDOR
            //DE LA IMAGEN DEL PERSONAJE
            string folder = this.helperPath.GetNameFolder(Folders.Imagenes);
            personaje.Imagen = this.helperPath.GetWebHostUrl()
                + folder  + "/" + fileName;
            await this.service.CreatePersonajeAsync(personaje.IdPersonaje
                , personaje.Nombre, personaje.Imagen, personaje.IdSerie);
            //LO VAMOS A LLEVAR A LA VISTA PERSONAJES SERIE
            return RedirectToAction("PersonajesSerie", new { idserie = personaje.IdSerie });
        }

        public async Task<IActionResult> UpdatePersonajeSerie()
        {
            List<Personaje> personajes = await this.service.GetPersonajesAsync();
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["PERSONAJES"] = personajes;
            ViewData["SERIES"] = series;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonajeSerie
            (int idpersonaje, int idserie)
        {
            await this.service.UpdateSeriePersonajeAsync(idpersonaje, idserie);
            return RedirectToAction("PersonajesSerie"
                , new { idserie = idserie });
        }
    }
}
