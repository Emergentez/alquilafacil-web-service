using AlquilaFacilPlatform.IAM.Domain.Model.Entities;
using AlquilaFacilPlatform.IAM.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.IAM.Domain.Repositories;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<bool> ExistsUserRole(EUserRoles role);
}