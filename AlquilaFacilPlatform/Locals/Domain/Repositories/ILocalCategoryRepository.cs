using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Domain.Repositories;

public interface ILocalCategoryRepository : IBaseRepository<LocalCategory>
{
    Task<bool> ExistsLocalCategory(EALocalCategoryTypes type);
    Task<IEnumerable<LocalCategory>> GetAllLocalCategories();
}