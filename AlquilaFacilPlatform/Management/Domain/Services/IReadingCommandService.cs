using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.Entities;

namespace AlquilaFacilPlatform.Management.Domain.Services;

public interface IReadingCommandService
{
    Task<Reading?> Handle(CreateReadingCommand command);
}