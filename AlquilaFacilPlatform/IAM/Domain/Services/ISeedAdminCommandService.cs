using AlquilaFacilPlatform.IAM.Domain.Model.Commands;

namespace AlquilaFacilPlatform.IAM.Domain.Services;

public interface ISeedAdminCommandService
{
    Task Handle(SeedAdminCommand command);
}