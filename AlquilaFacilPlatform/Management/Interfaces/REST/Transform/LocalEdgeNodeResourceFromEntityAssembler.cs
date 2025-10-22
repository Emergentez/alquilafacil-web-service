using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Management.Interfaces.REST.Transform;

public static class LocalEdgeNodeResourceFromEntityAssembler
{
    public static LocalEdgeNodeResource ToResourceFromEntity(LocalEdgeNode localEdgeNode)
    {
        return new LocalEdgeNodeResource(
            localEdgeNode.Id,
            localEdgeNode.LocalId,
            localEdgeNode.EdgeNodeUrl
        );
    }
}