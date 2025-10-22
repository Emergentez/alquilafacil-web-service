using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;

public interface ILocalExternalService
{
    Task<bool> LocalExists(int localId);
    Task<IEnumerable<Local?>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetOwnerIdByLocalId(int localId);
}