
using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface IReportCommandService
{
    Task<Report?> Handle(CreateReportCommand command);
    Task<Report?> Handle(DeleteReportCommand command);
}