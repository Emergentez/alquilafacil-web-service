namespace AlquilaFacilPlatform.Locals.Interfaces.REST.Resources;

public record CommentResource(int Id, int UserId, int LocalId, string Text, int Rating);