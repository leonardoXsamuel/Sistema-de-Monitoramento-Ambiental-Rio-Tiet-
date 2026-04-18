using ApsMartChat.DTOs.ChatRoom;

namespace ApsMartChat.Services.File;

public interface IChatRoomService
{
    public Task<ChatRoomResponseDTO> AlterarNomeChatRoom(int roomId, ChatRoomUpdateDTO chatRoomUpdateDTO);
}
