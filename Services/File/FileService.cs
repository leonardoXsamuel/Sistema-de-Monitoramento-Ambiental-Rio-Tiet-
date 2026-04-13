using ApsMartChat.DTOs;
using ApsMartChat.Models;
using EnviroChat.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ApsMartChat.Services.File;

public class FileService : IFileService
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    private static readonly HashSet<string> TiposArquivosPermitidos = new (StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    };

    private static readonly HashSet<string> ExtensoesArquivosPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".docx", ".xlsx"
    };

    private const long TamanhoMaxArqBytes = 50 * 1024 * 1024; // 50 MB

    public FileService(AppDbContext db, IWebHostEnvironment env)
    {
        _db  = db;
        _env = env;
    }

    public async Task<FileTransferDto> UploadAsync(
        IFormFile file, string username, int roomId, string baseUrl)
    {
        // ── Validações ───────────────────────────────────────────────────────
        if (file.Length > TamanhoMaxArqBytes)
            throw new InvalidOperationException("Arquivo excede 50 MB.");

        var ext = Path.GetExtension(file.FileName);
        if (!ExtensoesArquivosPermitidas.Contains(ext))
            throw new InvalidOperationException("Tipo de arquivo não permitido. Use .pdf, .docx ou .xlsx.");

        // ── Salvar no disco ──────────────────────────────────────────────────
        var uploadsPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsPath);

        var storedName = $"{Guid.NewGuid()}{ext}";
        var fullPath   = Path.Combine(uploadsPath, storedName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        // ── Salvar metadados no banco ────────────────────────────────────────
        var user = await _db.Users.FirstAsync(u => u.Username == username);

        var transfer = new FileTransfer
        {
            OriginalName = file.FileName,
            StoredName   = storedName,
            ContentType  = file.ContentType,
            SizeBytes    = file.Length,
            UploaderId   = user.Id,
            RoomId       = roomId
        };

        _db.FileTransfers.Add(transfer);
        await _db.SaveChangesAsync();

        return ToDto(transfer, user.DisplayName, baseUrl);
    }

    public async Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int fileId)
    {
        var transfer = await _db.FileTransfers.FindAsync(fileId);
        if (transfer is null) return null;

        var path = Path.Combine(
            _env.WebRootPath ?? "wwwroot", "uploads", transfer.StoredName);

        if (!File.Exists(path)) return null;

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return (stream, transfer.ContentType, transfer.OriginalName);
    }

    public async Task<List<FileTransferDto>> GetByRoomAsync(int roomId, string baseUrl)
    {
        return await _db.FileTransfers
            .Include(f => f.Uploader)
            .Where(f => f.RoomId == roomId)
            .OrderByDescending(f => f.UploadedAt)
            .Select(f => ToDto(f, f.Uploader.DisplayName, baseUrl))
            .ToListAsync();
    }

    private static FileTransferDto ToDto(FileTransfer f, string displayName, string baseUrl) =>
        new(f.Id, f.OriginalName, f.ContentType, f.SizeBytes, f.UploadedAt,
            displayName, $"{baseUrl}/api/files/{f.Id}/download");
}
