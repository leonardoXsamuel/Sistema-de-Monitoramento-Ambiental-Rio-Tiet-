namespace ApsMartChat.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public int RoomId { get; set; }
    public ChatRoom Room { get; set; } = null!;
}