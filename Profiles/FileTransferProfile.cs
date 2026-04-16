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
        CreateMap<FileTransfer, FileTransferResponseDTO>();
    }

}