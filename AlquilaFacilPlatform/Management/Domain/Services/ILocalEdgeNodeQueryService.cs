using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Management.Domain.Services;

public interface ILocalEdgeNodeQueryService
{
    Task<LocalEdgeNode?> Handle(GetLocalEdgeNodeByLocalIdQuery query);
}