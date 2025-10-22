using AlquilaFacilPlatform.IAM.Domain.Model.Commands;

namespace AlquilaFacilPlatform.IAM.Domain.Services;

public interface ISeedUserRoleCommandService
{
    Task Handle(SeedUserRolesCommand command);
}