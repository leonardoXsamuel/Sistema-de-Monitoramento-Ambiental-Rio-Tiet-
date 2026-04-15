using ApsMartChat.DTOs.ChatRoom;
using System.ComponentModel.DataAnnotations;

namespace ApsMartChat.DTOs.User;

public record MessageCreateDTO(
    [Required]
    [MinLength(1)]
    [StringLength(1000)]
    string Content,

    [Required]
    UserCreateDTO Sender,

    ChatRoomCreateDTO Room
);
    
