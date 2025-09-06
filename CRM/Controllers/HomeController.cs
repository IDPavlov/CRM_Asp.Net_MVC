using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CRM.Controllers
{
    [Route("")]
    [Route("{controller}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly CrmDbContext db;

        public HomeController(ILogger<HomeController> logger, CrmDbContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{action}")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("{action}")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
