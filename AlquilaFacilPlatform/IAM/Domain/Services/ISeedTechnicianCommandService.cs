using AlquilaFacilPlatform.IAM.Domain.Model.Commands;

namespace AlquilaFacilPlatform.IAM.Domain.Services;

public interface ISeedTechnicianCommandService
{
    Task Handle(SeedTechnicianCommand command);
}