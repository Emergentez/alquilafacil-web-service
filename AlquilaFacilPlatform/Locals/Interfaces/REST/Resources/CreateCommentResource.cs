namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

public record CreateCommentResource(int UserId, int LocalId, string Text, int Rating);