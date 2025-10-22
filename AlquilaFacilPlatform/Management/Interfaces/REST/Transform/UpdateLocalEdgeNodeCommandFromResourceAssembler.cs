using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Management.Interfaces.REST.Transform;

public static class UpdateLocalEdgeNodeCommandFromResourceAssembler
{
    public static UpdateLocalEdgeNodeCommand ToCommandFromResource(int localId, UpdateLocalEdgeNodeResource resource)
    {
        return new UpdateLocalEdgeNodeCommand(
            localId,
            resource.EdgeNodeUrl
        );
    }
}