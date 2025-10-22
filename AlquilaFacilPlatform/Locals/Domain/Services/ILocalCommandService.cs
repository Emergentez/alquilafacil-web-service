using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ILocalCommandService
{
    Task<Local?> Handle(CreateLocalCommand command);
    Task<Local?> Handle(UpdateLocalCommand command);
}