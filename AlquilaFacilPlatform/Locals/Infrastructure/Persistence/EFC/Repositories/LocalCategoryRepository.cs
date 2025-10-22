using System.ComponentModel;
using System.Reflection;
using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AlquilaFacilPlatform.Locals.Infrastructure.Persistence.EFC.Repositories;

public class LocalCategoryRepository(AppDbContext context)
    : BaseRepository<LocalCategory>(context), ILocalCategoryRepository
{
    public Task<bool> ExistsLocalCategory(EALocalCategoryTypes type)
    {
        var field = type.GetType().GetField(type.ToString());
        var descriptionAttributeName = ((DescriptionAttribute)field!.GetCustomAttribute(typeof(DescriptionAttribute))!).Description;
        return Context.Set<LocalCategory>().AnyAsync(x => x.Name == descriptionAttributeName);
    }

    public async Task<IEnumerable<LocalCategory>> GetAllLocalCategories()
    {
        return await Context.Set<LocalCategory>().ToListAsync();
    }
}
