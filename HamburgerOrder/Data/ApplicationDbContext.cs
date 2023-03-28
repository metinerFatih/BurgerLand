using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HamburgerOrder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var menus = new List<Menu>()
            {
                new Menu() {Id = 1, Name = "Whopper Menu", Price = 125},
                new Menu() {Id = 2, Name = "Chicken Menu", Price = 95}
            };
            modelBuilder.Entity<Menu>().HasData(menus);

            var extras = new List<Extra>()
            {
                new Extra() {Id = 1, Name = "Ranch Sauce", Price = 5},
                new Extra() {Id = 2, Name = "BBQ Sauce", Price = 5}
            };
            modelBuilder.Entity<Extra>().HasData(extras);
        }
    }
}