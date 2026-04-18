using ApsMartChat.DTOs.ChatRoom;
using ApsMartChat.Services.File;
using AutoMapper;
using EnviroChat.API.Data;

public class ChatRoomService : IChatRoomService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public ChatRoomService(AppDbContext db, IWebHostEnvironment env, IMapper mapper)
    {
        _db = db;
        _env = env;
        _mapper = mapper;
    }

    public async Task<ChatRoomResponseDTO> AlterarNomeChatRoom(int roomId, ChatRoomUpdateDTO chatRoomUpdateDTO)
    {
        var chatRoomExist = await _db.ChatRooms.FindAsync(roomId) ?? throw new Exception(); // => criar exceção personalizada para NotFound

        chatRoomExist.Name = chatRoomUpdateDTO.Name;
        await _db.SaveChangesAsync();

        return _mapper.Map<ChatRoomResponseDTO>(chatRoomExist);
    }
}
