using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace clone_oblt.Controllers
{
    public class HomeController : Controller
    {
        // This GET method helps us to get the index.html of our react app.
        // We send the folder and file name to the parameter to the indexhtml in the project.
        
        //In order to get the index.html => You have to create a front-end app first somewhere else that works as a standalone project.
        //After you're done developing the app, go to the console and type "npm run build"
        //This will produce the production codes for your front-end app.

        //Copy files in the "build" folder that has been created in your front-end app
        //Paste into wwwroot in your ASP.NET MVC app.

        //Done. That is how we use the react app in our MVC app.
        [HttpGet]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }
    }
}
