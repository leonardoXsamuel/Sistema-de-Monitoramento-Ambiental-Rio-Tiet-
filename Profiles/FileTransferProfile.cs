namespace ApsMartChat.Profiles;

using ApsMartChat.DTOs;
using ApsMartChat.DTOs.FileTransfer;
using ApsMartChat.Models;
using AutoMapper;

public class FileTransferProfile : Profile
{
    public FileTransferProfile()
    {
        CreateMap<FileTransferCreateDTO, FileTransfer>();
        CreateMap<FileTransferUpdateDTO, FileTransfer>();
        CreateMap<FileTransfer, FileTransferResponseDTO>()
            .ForMember(dest => dest.DownloadUrl,
                opt => opt.MapFrom(src => $"/api/files/{src.Id}/download"));
    }

}