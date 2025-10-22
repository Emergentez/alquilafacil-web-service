using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Interfaces.ACL;

namespace AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices.ExternalServices;

public class LocalExternalService(ILocalsContextFacade localsContextFacade) : ILocalExternalService
{
    public Task<bool> LocalExists(int reservationId)
    {
        return localsContextFacade.LocalExists(reservationId);
    }

    public async Task<IEnumerable<Local?>> GetLocalsByUserId(int userId)
    {
        return await localsContextFacade.GetLocalsByUserId(userId);
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        return await localsContextFacade.IsLocalOwner(userId, localId);
    }
    
    public async Task<int> GetOwnerIdByLocalId(int localId)
    {
        return await localsContextFacade.GetLocalOwnerIdByLocalId(localId);
    }
}