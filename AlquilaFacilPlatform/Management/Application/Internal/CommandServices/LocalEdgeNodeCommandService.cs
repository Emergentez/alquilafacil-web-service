using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Management.Application.Internal.CommandServices;

public class LocalEdgeNodeCommandService(
    ILocalEdgeNodeRepository localEdgeNodeRepository,
    ILocalExternalService localExternalService,
    IUnitOfWork unitOfWork
    ): ILocalEdgeNodeCommandService
{
    public async Task<LocalEdgeNode?> Handle(CreateLocalEdgeNodeCommand command)
    {
        if (!await localExternalService.LocalExists(command.LocalId))
        {
            throw new Exception("Local does not exist");
        }

        var localEdgeNode = new LocalEdgeNode(command);
        await localEdgeNodeRepository.AddAsync(localEdgeNode);
        await unitOfWork.CompleteAsync();
        return localEdgeNode;
    }
    public async Task<LocalEdgeNode?> Handle(UpdateLocalEdgeNodeCommand command)
    {
        var localEdgeNode = await localEdgeNodeRepository.GetByLocalIdAsync(command.LocalId);
        if (localEdgeNode == null)
        {
            throw new Exception("Local edge node not found");
        }

        if (!await localExternalService.LocalExists(command.LocalId))
        {
            throw new Exception("Local does not exist");
        }

        localEdgeNode.Update(command);
        localEdgeNodeRepository.Update(localEdgeNode);
        await unitOfWork.CompleteAsync();
        return localEdgeNode;
    }
}