using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AlquilaFacilPlatform.Management.Interfaces.REST.Hubs;

[Authorize]
public class ReadingHub : Hub
{
    public async Task JoinLocalGroup(int localId)
    {
        var groupName = GetGroupName(localId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveLocalGroup(int localId)
    {
        var groupName = GetGroupName(localId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    private string GetGroupName(int localId) => $"local:{localId}";
}