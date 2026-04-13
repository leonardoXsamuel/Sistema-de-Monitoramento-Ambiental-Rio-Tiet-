using ApsMartChat.DTOs;
using ApsMartChat.Services.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EnviroChat.API.Hubs;

/// <summary>
/// Hub SignalR — implementa os sockets de Berkeley indiretamente via TCP/IP.
/// O SignalR usa WebSockets (que encapsula TCP) como transporte preferencial,
/// caindo para Server-Sent Events ou Long Polling como fallback.
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    // ── Entrar em uma sala ────────────────────────────────────────────────────
    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, RoomGroup(roomId));
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("UserJoined", Context.User!.Identity!.Name, roomId);
    }

    // ── Sair de uma sala ──────────────────────────────────────────────────────
    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomGroup(roomId));
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("UserLeft", Context.User!.Identity!.Name, roomId);
    }

    // ── Enviar mensagem ───────────────────────────────────────────────────────
    public async Task SendMessage(string content, int roomId)
    {
        var username = Context.User!.Identity!.Name!;
        var msg = await _messageService.SaveAsync(content, username, roomId);

        // Broadcast para todos na sala (incluindo o remetente)
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("ReceiveMessage", msg);
    }

    // ── Notificar novo arquivo disponível ────────────────────────────────────
    public async Task NotifyFileUploaded(FileTransferDto file, int roomId)
    {
        await Clients.Group(RoomGroup(roomId))
            .SendAsync("FileAvailable", file);
    }

    // ── Indicador de digitação ────────────────────────────────────────────────
    public async Task Typing(int roomId, bool isTyping)
    {
        var username = Context.User!.Identity!.Name;
        await Clients.OthersInGroup(RoomGroup(roomId))
            .SendAsync("UserTyping", username, isTyping);
    }

    // ── Desconexão ────────────────────────────────────────────────────────────
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    private static string RoomGroup(int roomId) => $"room_{roomId}";
}
