using ApsMartChat.DTOs.ChatRoom;
using ApsMartChat.Services.File;
using Microsoft.AspNetCore.Mvc;

namespace ApsMartChat.Controllers;

public class ChatRoomController : ControllerBase
{
    private readonly IChatRoomService _service;

    public ChatRoomController(IChatRoomService service) => _service = service;

    [HttpPut("updateChatRoom/{roomId}")]
    public async Task<ActionResult<ChatRoomResponseDTO>> AlterarNomeChatRoom([FromRoute] int roomId, [FromBody] ChatRoomUpdateDTO chatRoomUpdateDTO)
    {
        var dto = await _service.AlterarNomeChatRoom(roomId, chatRoomUpdateDTO);

        if (dto is null)
            return BadRequest();

        return Ok(dto);
    }



}
