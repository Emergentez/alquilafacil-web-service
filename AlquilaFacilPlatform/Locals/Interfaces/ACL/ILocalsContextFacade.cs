using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Locals.Interfaces.ACL;

public interface ILocalsContextFacade
{

    Task<bool> LocalExists(int localId);
    
    Task<IEnumerable<Local?>> GetLocalsByUserId(int userId);
    
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetLocalOwnerIdByLocalId(int localId);
}