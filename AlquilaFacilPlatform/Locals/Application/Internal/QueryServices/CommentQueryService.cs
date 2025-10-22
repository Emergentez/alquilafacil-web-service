using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;

namespace AlquilaFacilPlatform.Locals.Application.Internal.QueryServices;

public class CommentQueryService (ICommentRepository commentRepository) : ICommentQueryService
{
    public Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query)
    {
        return commentRepository.GetAllCommentsByLocalId(query.LocalId);
    }
}