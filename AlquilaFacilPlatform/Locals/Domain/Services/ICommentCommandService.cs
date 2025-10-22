using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ICommentCommandService
{
    Task<Comment?> Handle(CreateCommentCommand command);
}