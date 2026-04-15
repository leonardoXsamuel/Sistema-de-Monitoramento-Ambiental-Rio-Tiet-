using ApsMartChat.DTOs.FileTransfer;
using ApsMartChat.DTOs.Message;
using ApsMartChat.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace ApsMartChat.DTOs.ChatRoom;

public record ChatRoomCreateDTO( [Required] [StringLength(155, MinimumLength = 4)] string Name);
