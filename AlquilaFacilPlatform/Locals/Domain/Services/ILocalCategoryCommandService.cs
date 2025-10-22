using AlquilaFacilPlatform.Locals.Domain.Model.Commands;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ILocalCategoryCommandService
{
    Task Handle(SeedLocalCategoriesCommand command);
}