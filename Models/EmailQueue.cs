namespace SIGES.Models
{
    public class EmailQueue
    {
        public int Id { get; set; }
        public string To { get; set; } = string.Empty;
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Headers { get; set; }
    }
}
