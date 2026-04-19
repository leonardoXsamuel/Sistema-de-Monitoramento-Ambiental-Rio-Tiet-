using ApsMartChat.DTOs.ChatRoom;
using ApsMartChat.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace ApsMartChat.DTOs.FileTransfer;

public record FileTransferUpdateDTO([Required] [StringLength(55), MinLength(1)] string NomeOriginal);