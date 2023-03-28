namespace HamburgerOrder.Data
{
    public class Order
    {
        public Order()
        {
            TotalPrice = 0;
        }
        public int Id { get; set; }
        public Menu SelectedMenu { get; set; } = new Menu();

        public List<Extra> Extras { get; set; } = new List<Extra>();

        public SizeEnum Size { get; set; }

        public int Amount { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public void Calculate()
        {
            if (SelectedMenu != null)
            {
                TotalPrice = 0;
                TotalPrice += SelectedMenu.Price;

                switch (Size)
                {
                    case SizeEnum.Medium:
                        TotalPrice += TotalPrice * 0.1M;
                        break;
                    case SizeEnum.Large:
                        TotalPrice += TotalPrice * 0.2M;
                        break;
                }

                TotalPrice *= Amount;

                foreach (Extra item in Extras)
                {
                    TotalPrice += item.Price;
                }
            }
        }
    }
}
