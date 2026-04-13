using ApsMartChat.DTOs.ChatRoom;
using ApsMartChat.DTOs.FileTransfer;

namespace ApsMartChat.DTOs.ChatRoom;

public record ChatRoomUpdateDTO
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<MessageCreateDTO> Messages { get; set; } = new List<MessageCreateDTO>();
    public ICollection<FileTransferCreateDTO> FileTransfers { get; set; } = new List<FileTransferCreateDTO>();
}
