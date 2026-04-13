namespace ApsMartChat.Models;

public class ChatRoom
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<FileTransfer> FileTransfers { get; set; } = new List<FileTransfer>();
}
