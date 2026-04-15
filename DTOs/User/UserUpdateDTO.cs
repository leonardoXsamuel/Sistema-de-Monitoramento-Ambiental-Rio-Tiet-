using System.ComponentModel.DataAnnotations;

namespace ApsMartChat.DTOs.User;

public record UserUpdateDTO(

    [StringLength(100, MinimumLength = 3)]
    string? Username,

    [StringLength(100, MinimumLength = 3)]
    string? DisplayName
);