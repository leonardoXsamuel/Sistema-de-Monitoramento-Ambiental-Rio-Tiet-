using EnviroChat.API.Models;
using EnviroChat.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApsMartChat.Controllers;

[Authorize]
[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileService _files;

    public FilesController(IFileService files) => _files = files;

    /// <summary>Upload de arquivo para uma sala. Aceita .pdf, .docx, .xlsx (max 50 MB).</summary>
    [HttpPost("upload")]
    [RequestSizeLimit(52_428_800)] // 50 MB
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] int roomId)
    {
        try
        {
            var username = User.Identity!.Name!;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var dto = await _files.UploadAsync(file, username, roomId, baseUrl);
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Download de um arquivo pelo ID.</summary>
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var result = await _files.DownloadAsync(id);
        if (result is null)
            return NotFound(new { message = "Arquivo não encontrado." });

        var (stream, contentType, fileName) = result;
        return File(stream, contentType, fileName);
    }

    /// <summary>Lista arquivos de uma sala.</summary>
    [HttpGet("room/{roomId:int}")]
    public async Task<IActionResult> GetByRoom(int roomId)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var files = await _files.GetByRoomAsync(roomId, baseUrl);
        return Ok(files);
    }
}

