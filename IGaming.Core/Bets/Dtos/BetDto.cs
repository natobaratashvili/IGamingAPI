
namespace IGaming.Core.Bets.Dtos
{
    public class BetDto
    {

        public string? Details { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
        public DateTime? CreatedAtUtc { get; set; }
    }
}
