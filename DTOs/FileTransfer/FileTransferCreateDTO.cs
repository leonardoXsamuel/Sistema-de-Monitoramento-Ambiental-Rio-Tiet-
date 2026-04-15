using ApsMartChat.DTOs.ChatRoom;
using ApsMartChat.DTOs.User;

namespace ApsMartChat.DTOs.FileTransfer;

public record FileTransferCreateDTO
{
    public string NomeOriginal { get; set; } = string.Empty;  // nome original
    public string NomeGeradoCript { get; set; } = string.Empty;  // nome q vai ser criado no FileService 
    public string TipoConteudo { get; set; } = string.Empty;  // ex: application/pdf
    public long TamanhoBytes { get; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public int UploaderId { get; set; }
    public UserCreateDTO Uploader { get; set; } = null!;

    public int RoomId { get; set; }
    public ChatRoomCreateDTO Room { get; set; } = null!;

}
