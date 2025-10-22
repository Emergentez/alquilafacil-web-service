using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Domain.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
        Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId);
}