using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Management.Infrastructure.Persistence.EFC.Repositories;

public class LocalEdgeNodeRepository(AppDbContext context): BaseRepository<LocalEdgeNode>(context), ILocalEdgeNodeRepository
{
    public async Task<LocalEdgeNode?> GetByLocalIdAsync(int localId)
    {
        return await Context.Set<LocalEdgeNode>().FirstOrDefaultAsync(node => node.LocalId == localId);
    }
}