using ApsMartChat.Services.File;
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
    private readonly FileService _files;

    public FilesController(FileService files) => _files = files;

    ///Upload de arquivo para uma sala. Aceita .pdf, .docx, .xlsx (max 200 MB).
    [HttpPost("upload")]
    [RequestSizeLimit(52_428_800)] // 200 MB => ajustar anotacao pra receber 200MB ao invés de 50MB
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] int roomId)
    {
        try
        {
            var username = User.Identity!.Name!;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var dto = await _files.UploadDeArquivoAsync(file, username, roomId, baseUrl);
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// Download de um arquivo pelo ID.
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var result = await _files.DownloadDeArquivoAsync(id);
        if (result is null)
            return NotFound(new { message = "Arquivo não encontrado." });

        var (stream, contentType, fileName) = result;
        return File(stream, contentType, fileName);
    }

    /// Lista arquivos de uma sala.
    [HttpGet("room/{roomId:int}")]
    public async Task<IActionResult> GetByRoom(int roomId)
    {
        //var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var files = await _files.GetFilesByRoomAsync(roomId);
        return Ok(files);
    }
}

