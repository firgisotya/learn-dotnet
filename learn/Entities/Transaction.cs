namespace learn.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }

    }
}
