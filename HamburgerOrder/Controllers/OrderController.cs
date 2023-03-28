using HamburgerOrder.Data;
using HamburgerOrder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HamburgerOrder.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
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
        public IActionResult OrderHistory()
        {
            if (_db.Orders.Include(o => o.SelectedMenu).Where(x => x.UserId == UserId).Count() <= 0)
            {
                ViewBag.Order = "There is no order.";
            }

            var orders = _db.Orders.Include(s => s.SelectedMenu).Where(x => x.UserId == UserId).ToList();

            return View(orders);
        }
        public IActionResult CreateOrder()
        {
            var order = new Order();
            ViewBag.Menus = new SelectList(_db.Menus, "Id", "Name");
            ViewBag.Extras = _db.Extras.ToList();

            order.Calculate();
            return View(order);
        }
        [HttpPost]
        public IActionResult CreateOrder(Order order, List<int> extras)
        {

            order.SelectedMenu = _db.Menus.FirstOrDefault(x => x.Id == order.SelectedMenu.Id);
            order.Extras = _db.Extras.Where(x => extras.Contains(x.Id)).ToList();
            order.UserId = UserId;
            order.Calculate();

            _db.Orders.Add(order);
            _db.SaveChanges();
            return View("OrderInfo", order);

        }
        public IActionResult OrderInfo(Order order, List<int> extras)
        {
            return View(order);
        }
        [HttpPost]
        public IActionResult OrderInfo()
        {
            return RedirectToAction("Index");
        }

        public IActionResult UpdateOrder(int id)
        {
            var order = _db.Orders.Include(o => o.SelectedMenu).Include(o => o.Extras).FirstOrDefault(x => x.Id == id);
            ViewBag.Menus = new SelectList(_db.Menus, "Id", "Name");
            ViewBag.Extras = _db.Extras.ToList();

            return View(order);
        }
        [HttpPost]
        public IActionResult UpdateOrder(Order order, List<int> extras)
        {
            var updatedOrder = _db.Orders.Include(o => o.SelectedMenu).Include(o => o.Extras).FirstOrDefault(x => x.Id == order.Id);
            updatedOrder.SelectedMenu = _db.Menus.Find(order.SelectedMenu.Id);
            updatedOrder.Extras = _db.Extras.Where(e => extras.Contains(e.Id)).ToList();
            updatedOrder.Size = order.Size;
            updatedOrder.Amount = order.Amount;
            updatedOrder.Calculate();

            _db.Update(updatedOrder);
            _db.SaveChanges();

            return View("OrderInfo", updatedOrder);
        }

        public IActionResult DeleteOrder(int id)
        {
            var order = _db.Orders.Include(o => o.SelectedMenu).Include(o => o.Extras).FirstOrDefault(x => x.Id == id);

            _db.Orders.Remove(order);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
