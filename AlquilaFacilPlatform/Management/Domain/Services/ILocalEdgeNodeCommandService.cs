using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.Entities;

namespace AlquilaFacilPlatform.Management.Domain.Services;

public interface ILocalEdgeNodeCommandService
{
    Task<LocalEdgeNode?> Handle(CreateLocalEdgeNodeCommand command);
    Task<LocalEdgeNode?> Handle(UpdateLocalEdgeNodeCommand command);
}