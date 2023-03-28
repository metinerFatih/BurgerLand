using HamburgerOrder.Data;
using HamburgerOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburgerOrder.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var products = new ProductsViewModel()
            {
                Menus = _db.Menus.ToList(),
                Extras = _db.Extras.ToList()
            };

            return View(products);
        }

        public IActionResult AddMenu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMenu(MenuViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu()
                {
                    Name = vm.Name,
                    Price = vm.Price
                };

                _db.Menus.Add(menu);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View();
        }
        public IActionResult UpdateMenu(int id)
        {
            var menu = _db.Menus.Find(id);
            if (menu == null)
                return NotFound();

            MenuViewModel vm = new MenuViewModel()
            {
                Name = menu.Name,
                Price = menu.Price
            };
            TempData["menu"] = id;
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateMenu(MenuViewModel vm)
        {
            int id = Convert.ToInt32(TempData["menu"]);
            var menu = _db.Menus.Find(id);

            if(menu is not null && ModelState.IsValid)
            {
                menu.Name = vm.Name;
                menu.Price = vm.Price;

                _db.Menus.Update(menu);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult DeleteMenu(int id)
        {
            var menu = _db.Menus.Find(id);
            _db.Menus.Remove(menu);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult AddExtra()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExtra(ExtraViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Extra extra = new Extra()
                {
                    Name = vm.Name,
                    Price = vm.Price
                };

                _db.Extras.Add(extra);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult UpdateExtra(int id)
        {
            var extra = _db.Extras.Find(id);
            if (extra == null)
                return NotFound();

            ExtraViewModel vm = new ExtraViewModel()
            {
                Name = extra.Name,
                Price = extra.Price
            };
            TempData["extra"] = id;
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateExtra(ExtraViewModel vm)
        {
            int id = Convert.ToInt32(TempData["extra"]);
            var extra = _db.Extras.Find(id);

            if (extra is not null && ModelState.IsValid)
            {
                extra.Name = vm.Name;
                extra.Price = vm.Price;

                _db.Extras.Update(extra);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult DeleteExtra(int id)
        {
            var extra = _db.Extras.Find(id);
            _db.Extras.Remove(extra);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult UserOrders()
        {
            var userOrders = _db.Orders.Include(o => o.SelectedMenu).Include(o => o.User).ToList();
            return View(userOrders);
        }
    }
}
