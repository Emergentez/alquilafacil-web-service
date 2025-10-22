using AlquilaFacilPlatform.Management.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Management.Domain.Model.Entities;

public class LocalEdgeNode
{
    public LocalEdgeNode()
    {
        LocalId = 0;
        EdgeNodeUrl = string.Empty;
    }
    
    public LocalEdgeNode(int localId, string edgeNodeUrl)
    {
        LocalId = localId;
        EdgeNodeUrl = edgeNodeUrl;
    }
    
    public LocalEdgeNode(CreateLocalEdgeNodeCommand command)
    {
        LocalId = command.LocalId;
        EdgeNodeUrl = command.EdgeNodeUrl;
    }
    
    public void Update(UpdateLocalEdgeNodeCommand command)
    {
        EdgeNodeUrl = command.EdgeNodeUrl;
    }
    
    public int Id { get; set; }
    public int LocalId { get; set; }
    public string EdgeNodeUrl { get; set; } = string.Empty;
}