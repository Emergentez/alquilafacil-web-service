using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Management.Interfaces.REST.Transform;

public static class CreateLocalEdgeNodeCommandFromResourceAssembler
{
    public static CreateLocalEdgeNodeCommand ToCommandFromResource(CreateLocalEdgeNodeResource resource)
    {
        return new CreateLocalEdgeNodeCommand(
            resource.LocalId,
            resource.EdgeNodeUrl
        );
    }
}