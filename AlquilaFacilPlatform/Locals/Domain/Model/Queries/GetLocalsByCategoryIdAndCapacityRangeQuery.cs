namespace AlquilaFacilPlatform.Locals.Domain.Model.Queries;

public record GetLocalsByCategoryIdAndCapacityRangeQuery(int LocalCategoryId, int MinCapacity, int MaxCapacity);