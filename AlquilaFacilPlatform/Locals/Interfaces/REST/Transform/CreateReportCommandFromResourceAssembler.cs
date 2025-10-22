using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;

public class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        return new CreateReportCommand(
             resource.LocalId,
             resource.Title,
             resource.UserId,
             resource.Description
        );
    }
}