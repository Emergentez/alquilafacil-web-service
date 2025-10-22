namespace AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;

public record CreatePlanCommand(string Name, string Service, float Price);