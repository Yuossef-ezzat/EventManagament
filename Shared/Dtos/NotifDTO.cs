namespace Shared.Dtos;

public class NotifDTO
{
    public int NotifId { get; set; }
    public required string Message { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required string UserId { get; set; }
    public string? UserName { get; set; }
    public bool IsRead { get; set; } = false;
}
