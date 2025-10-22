using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Model.Entities;
using AlquilaFacilPlatform.IAM.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.IAM.Application.Internal.CommandServices;

public class SeedUserRoleCommandService(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ISeedUserRoleCommandService
{
    public async Task Handle(SeedUserRolesCommand command)
    {
        foreach (EUserRoles role in Enum.GetValues(typeof(EUserRoles)))
        {
            if (await repository.ExistsUserRole(role)) continue;
            var userRole = new UserRole(role.ToString());
            await repository.AddAsync(userRole);
        }

        await unitOfWork.CompleteAsync();
    }
}