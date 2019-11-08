using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Phantom_Games.Models;

namespace Phantom_Games.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Erro(string mensagem)
        {
            return new ContentResult()
            {
                Content = mensagem,
                ContentType = "text/plain",
                StatusCode = 500
            };
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Preferences()
        {
            return View();
        }
        public IActionResult Personality()
        {
            return View();
        }

        public IActionResult Opcoes() {
            return View();
        }

        public IActionResult GoToRegister()
        {
            return RedirectToAction("Register", "Login");

        }
        public IActionResult GoToSaveResults()
        {
            return RedirectToAction("SaveResults", "Login");

        }

        //COISA DE DELETAR O USUÁRIO
        //[HttpPost]
        //public IActionResult Deletar([FromForm] Login user)
        //{
        //    using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=banco_site;Uid=root;Pwd=root;"))
        //    {
        //        conn.Open();

        //    }

        //    return View("Index");
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     

    }
}
