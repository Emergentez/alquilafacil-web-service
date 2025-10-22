using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Model.Entities;
using AlquilaFacilPlatform.Management.Domain.Model.Events;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Management.Interfaces.REST.Hubs;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace AlquilaFacilPlatform.Management.Application.Internal.CommandServices;

public class ReadingCommandService(
    IReadingRepository readingRepository,
    ISensorTypeRepository sensorTypeRepository,
    ILocalExternalService localExternalService,
    IUnitOfWork unitOfWork,
    IHubContext<ReadingHub> hubContext
    ): IReadingCommandService
{
    public async Task<Reading?> Handle(CreateReadingCommand command)
    {
        if (!await localExternalService.LocalExists(command.LocalId))
        {
            throw new Exception("Local does not exist");
        }
        var sensorType = await sensorTypeRepository.FindByIdAsync(command.SensorTypeId);
        if (sensorType == null)
        {
            throw new Exception("Sensor type not found");
        }
        var reading = new Reading(command);
        await readingRepository.AddAsync(reading);
        await unitOfWork.CompleteAsync();
        
        var readingReceivedEvent = new ReadingReceivedEvent(
            reading.LocalId,
            reading.SensorTypeId,
            reading.Message
        );
        
        await hubContext.Clients.Group(GetGroupName(reading.LocalId)).SendAsync("ReadingReceivedEvent", readingReceivedEvent);
        
        return reading;
    }
    
    private static string GetGroupName(int localId) => $"local:{localId}";

}