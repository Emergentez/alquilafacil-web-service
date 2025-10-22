using System.ComponentModel;
using System.Reflection;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Application.Internal.CommandServices;

public class LocalCategoryCommandService(ILocalCategoryRepository localCategoryRepository, IUnitOfWork unitOfWork) : ILocalCategoryCommandService
{
    
    public async Task Handle(SeedLocalCategoriesCommand command)
    {
        var imageUrls = new Dictionary<EALocalCategoryTypes, string>
        {
            { EALocalCategoryTypes.BeachHouse, "https://tse4.mm.bing.net/th?id=OIP.N62R-B5j13QHIL9OhcdJ1wHaHa&pid=Api&P=0&h=180" },
            { EALocalCategoryTypes.LandscapeHouse, "https://cdn-icons-png.flaticon.com/512/74/74951.png" },
            { EALocalCategoryTypes.CityHouse, "https://tse1.mm.bing.net/th?id=OIP.LnALBZ2Bu7Mnw46vjKeMYAHaHa&pid=Api&P=0&h=180" },
            { EALocalCategoryTypes.ElegantRoom, "https://cdn3.iconfinder.com/data/icons/beauty-cosmetics-1-line/128/beauty-salon_beauty_salon_barbershop_glamour-512.png" }
        };

        foreach (EALocalCategoryTypes type in Enum.GetValues(typeof(EALocalCategoryTypes)))
        {
            if (!await localCategoryRepository.ExistsLocalCategory(type))
            {
                var field = type.GetType().GetField(type.ToString());
                if (field == null) continue;
        
                var attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute))!;
                var attributeDescription = attribute.Description;

                if (!imageUrls.TryGetValue(type, out var photoUrl))
                {
                    throw new Exception("Invalid local category type");
                }

                var localCategory = new LocalCategory(attributeDescription, photoUrl);
                await localCategoryRepository.AddAsync(localCategory);
            }
        }
        await unitOfWork.CompleteAsync();
    }
}
