using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Locals.Domain.Services;

namespace AlquilaFacilPlatform.Locals.Interfaces.ACL.Services;

public class LocalsContextFacade(ILocalQueryService localQueryService) : ILocalsContextFacade
{
    public async Task<bool> LocalExists(int localId)
    {
        var query = new GetLocalByIdQuery(localId);
        var local = await localQueryService.Handle(query);
        if (local == null)
        {
            throw new Exception("Local not found");
        }

        return true;
    }

    public async Task<IEnumerable<Local?>> GetLocalsByUserId(int userId)
    {
        var query = new GetLocalsByUserIdQuery(userId);
        var locals = await localQueryService.Handle(query);
        if (locals == null)
        {
            throw new Exception("Local doesnt exists");
        }
        return locals;
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        var query = new IsLocalOwnerQuery(userId, localId);
        var isOwner = await localQueryService.Handle(query);
        return isOwner;
    }
    
    public async Task<int> GetLocalOwnerIdByLocalId(int localId)
    {
        var query = new GetLocalOwnerIdByLocalId(localId);
        var ownerId = await localQueryService.Handle(query);
        return ownerId;
    }
}