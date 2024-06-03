namespace WebApplication1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; }
        public int? FromUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public User FromUser { get; set; }
        public decimal Remain {  get; set; }
    }
}
