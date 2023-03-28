using Microsoft.AspNetCore.Identity;
using HamburgerOrder.Data;

namespace HamburgerOrder
{
    public class ApplicationUser : IdentityUser
    {
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
