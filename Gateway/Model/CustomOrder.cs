namespace Gateway.Model
{
    public class CustomOrder
    {
        public int Id { get; set; }
        public double TotalMoney { get; set; }
        public List<Product> Products { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
