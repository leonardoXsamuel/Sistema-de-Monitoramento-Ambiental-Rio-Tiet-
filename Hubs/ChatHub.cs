using ApsMartChat.DTOs.FileTransfer;
using ApsMartChat.Services.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ApsMartChat.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMessageService messageService, ILogger<ChatHub> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    // padroniza o nome do grupo
    private static string RoomGroup(int roomId) => $"room_{roomId}";

    public async Task EntrarNoChatRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, RoomGroup(roomId));
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("UsuarioEntrou", $"Usuario{Context.User?.Identity?.Name ?? "Anônimo"} entrou na sala {roomId}");
    }

    public async Task SairDoChatRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomGroup(roomId));
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("UsuarioSaiu", $"Usuario{Context.User?.Identity?.Name ?? "Anônimo"} saiu da sala {roomId}");
    }

    //  Enviar mensagem 
    public async Task EnviarMensagem(string content, int roomId)
    {
        var username = Context.User!.Identity!.Name!;
        var msg = await _messageService.SaveMessageAsync(content, username, roomId);

        // Enviando a msg para todos na sala (incluindo o remetente)
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("ReceberMensagem", msg);
    }

    //  Notificar novo arquivo disponível 
    public async Task NotificaçãoDeArquivoCarregado(FileTransferResponseDTO file, int roomId)
    {
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("ArquivoDisponível", file);
    }

    //  Indicador de digitação 
    public async Task Digitando(int roomId, bool isTyping)
    {
        var username = Context.User!.Identity!.Name;
        await Clients.OthersInGroup(RoomGroup(roomId))
            .SendAsync("UsuárioDigitando", $"Usuario{username ?? "Anônimo"} está digitando", isTyping);
    }

    //  Desconexão da sala
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User?.Identity?.Name ?? "Anonimo";

        if (exception is null)
        {
            _logger.LogInformation("Usuário {Username} se desconectou", username);
        }
        else
        {
            // opcional: log de erro
            _logger.LogError(exception, "Erro na desconexão do usuário {Username}", username);
        }

        await base.OnDisconnectedAsync(exception);
    }

}
