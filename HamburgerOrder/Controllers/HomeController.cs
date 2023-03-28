﻿using HamburgerOrder.Data;
using HamburgerOrder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HamburgerOrder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = new ProductsViewModel()
            {
                Extras = _db.Extras.ToList(),
                Menus = _db.Menus.ToList()
            };

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}