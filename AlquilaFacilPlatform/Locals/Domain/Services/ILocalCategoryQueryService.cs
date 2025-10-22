using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Locals.Domain.Services;

public interface ILocalCategoryQueryService
{
    Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query);
}
