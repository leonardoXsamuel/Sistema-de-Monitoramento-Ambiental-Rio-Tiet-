using ApsMartChat.DTOs.FileTransfer;

namespace ApsMartChat.DTOs.ChatRoom;

public record ChatRoomResponseDTO(
    string Name,
    DateTime CreatedAt,

    ICollection<MessageCreateDTO> Messages,
    ICollection<FileTransferCreateDTO> FileTransfers
);
