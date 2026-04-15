using ApsMartChat.DTOs.FileTransfer;
using ApsMartChat.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace ApsMartChat.DTOs.ChatRoom;

public record UserCreateDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string DisplayName { get; set; } 

}
