using System.Reflection.Metadata;
using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Locals.Infrastructure.Persistence.EFC.Repositories;

public class CommentRepository(AppDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId)
    {
        return await Context.Set<Comment>().Where(c => c.LocalId == localId).ToListAsync();
    }
}