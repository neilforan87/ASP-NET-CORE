using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WorldJourney.Models;
using WorldJourney.Filters;


namespace WorldJourney.Controllers
{
    public class CityController : Controller
    {
        private IData _data;
        private IHostingEnvironment _environment;

        public CityController(IData data, IHostingEnvironment environment)
        {
            _data = data;
            environment = _environment;
            _data.CityInitializeData();
        }

        [ServiceFilter(typeof(LogActionFilterAttribute))]
        [Route("WorldJourney")]
        public IActionResult Index()
        {
            ViewData["Page"] = "Search City";
            return View();
        }

        public IActionResult Details(int? Id)
        {
            ViewData["Page"] = "Selected City";
            City city = _data.GetCityById(Id);

            if(city == null)
            {
                return NotFound();
            }

            ViewBag.Title = city.CityName;
            return View(city);
        }

        public IActionResult GetImage(int? CityID)
        {
            ViewData["Message"] = "Display Image";
            
            City requestedCity = _data.GetCityById(CityID);

            if(requestedCity != null)
            {
                string webRootPath = _environment.WebRootPath;
                string folderPath = "\\images\\";

                string fullPath = webRootPath + folderPath + requestedCity.ImageName;

                FileStream fileOnDisk = new FileStream(fullPath, FileMode.Open);
                byte[] fileBytes;

                using (BinaryReader br =  new BinaryReader(fileOnDisk))
                {
                    fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                }
                return File(fileBytes, requestedCity.ImageMimeType);
            }
            else
            {
                return NotFound();
            }

           

            
        }
    }
}