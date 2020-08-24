using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace GoingPlaces.API.Controllers
{
    public class Fallback : Controller
    {
        // We need this method to serve Angular files
        // And it something to do with Angular routing
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            "index.html"), "text/HTML");
        }
    }
}