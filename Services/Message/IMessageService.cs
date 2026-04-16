using ApsMartChat.DTOs.Message;

namespace ApsMartChat.Services.Message;

public interface IMessageService
{
    Task<MessageResponseDTO> SaveAsync(string content, string username, int roomId);
    Task<List<MessageResponseDTO>> GetHistoryAsync(int roomId, int page = 1, int pageSize = 50);
}