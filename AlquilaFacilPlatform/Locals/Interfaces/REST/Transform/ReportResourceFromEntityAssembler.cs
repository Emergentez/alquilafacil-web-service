using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report? report)
    {
        return new ReportResource(
            report.Id,
            report.LocalId,
            report.Title,
            report.UserId,
            report.Description
        );
    }
}