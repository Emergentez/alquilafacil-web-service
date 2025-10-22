using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ICommentQueryService
{
    Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query);
}