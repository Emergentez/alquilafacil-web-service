using AlquilaFacilPlatform.IAM.Domain.Model.Entities;
using AlquilaFacilPlatform.IAM.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.IAM.Infrastructure.Persistence.EFC.Respositories;

public class UserRoleRepository(AppDbContext context) : BaseRepository<UserRole>(context), IUserRoleRepository
{
    public async Task<bool> ExistsUserRole(EUserRoles role)
    {
        return await Context.Set<UserRole>().AnyAsync(userRole => userRole.Role == role.ToString());
    }
}
