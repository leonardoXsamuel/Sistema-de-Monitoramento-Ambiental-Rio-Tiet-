using ApsMartChat.DTOs;
using EnviroChat.API.Data;
using EnviroChat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ApsMartChat.Services.Message;

public interface IMessageService
{
    Task<MessageDto> SaveAsync(string content, string username, int roomId);
    Task<List<MessageDto>> GetHistoryAsync(int roomId, int page = 1, int pageSize = 50);
}

public class MessageService : IMessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db) => _db = db;

    public async Task<MessageDto> SaveAsync(string content, string username, int roomId)
    {
        var user = await _db.Users.FirstAsync(u => u.Username == username);

        var message = new Message
        {
            Content  = content,
            SenderId = user.Id,
            RoomId   = roomId,
            SentAt   = DateTime.UtcNow
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        return new MessageDto(
            message.Id,
            message.Content,
            user.Username,
            user.DisplayName,
            message.SentAt,
            roomId
        );
    }

    public async Task<List<MessageDto>> GetHistoryAsync(int roomId, int page = 1, int pageSize = 50)
    {
        return await _db.Messages
            .Include(m => m.Sender)
            .Where(m => m.RoomId == roomId)
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MessageDto(
                m.Id,
                m.Content,
                m.Sender.Username,
                m.Sender.DisplayName,
                m.SentAt,
                m.RoomId
            ))
            .ToListAsync();
    }
}
