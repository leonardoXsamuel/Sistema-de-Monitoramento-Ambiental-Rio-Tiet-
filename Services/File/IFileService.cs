using ApsMartChat.DTOs;

namespace ApsMartChat.Services.File;

public interface IFileService
{
    Task<FileTransferDto> UploadAsync(IFormFile file, string username, int roomId, string baseUrl);
    Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int fileId);
    Task<List<FileTransferDto>> GetByRoomAsync(int roomId, string baseUrl);
}
