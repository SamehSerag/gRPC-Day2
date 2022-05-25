namespace PaymentDemo.Model
{
    public class User
    {
        public User(int id, string name, double balance)
        {
            Id = id;
            this.name = name;
            Balance = balance;
        }

        public int Id { get; set; }
        public string name { get; set; }
        public double Balance { get; set; }

    }
}
