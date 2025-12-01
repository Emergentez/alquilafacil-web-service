using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Model.Entities;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Shared.Domain.Repositories;

namespace AlquilaFacilPlatform.Locals.Application.Internal.CommandServices;

public class SeedDemoLocalsCommandService(
    ILocalRepository localRepository,
    IUnitOfWork unitOfWork) : ISeedDemoLocalsCommandService
{
    public async Task Handle(SeedDemoLocalsCommand command)
    {
        var existingLocals = await localRepository.GetLocalsAsync();
        if (existingLocals.Any()) return;

        var demoLocals = new List<(CreateLocalCommand Command, List<string> Photos)>
        {
            (
                new CreateLocalCommand(
                    "Casa de Playa Miraflores",
                    "Hermosa casa frente al mar con vista panorámica, ideal para eventos y reuniones familiares.",
                    "Perú",
                    "Lima",
                    "Miraflores",
                    "Av. Malecón de la Reserva 610",
                    150,
                    50,
                    new List<string> { "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?w=800" },
                    "WiFi,Estacionamiento,Piscina,Jardín,Parrilla",
                    1,
                    1
                ),
                new List<string> { "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?w=800" }
            ),
            (
                new CreateLocalCommand(
                    "Salón Elegante San Isidro",
                    "Salón de eventos premium con decoración moderna, perfecto para bodas y eventos corporativos.",
                    "Perú",
                    "Lima",
                    "San Isidro",
                    "Calle Los Laureles 250",
                    200,
                    100,
                    new List<string> { "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800" },
                    "WiFi,Aire Acondicionado,Proyector,Sistema de Sonido,Catering",
                    4,
                    1
                ),
                new List<string> { "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=800" }
            ),
            (
                new CreateLocalCommand(
                    "Casa Campo Cieneguilla",
                    "Amplia casa de campo con áreas verdes, ideal para retiros y celebraciones al aire libre.",
                    "Perú",
                    "Lima",
                    "Cieneguilla",
                    "Km 25 Carretera a Cieneguilla",
                    120,
                    80,
                    new List<string> { "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800" },
                    "Estacionamiento,Piscina,Jardín,Parrilla,Cancha de Fútbol",
                    2,
                    1
                ),
                new List<string> { "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800" }
            ),
            (
                new CreateLocalCommand(
                    "Loft Urbano Barranco",
                    "Moderno loft en el corazón de Barranco, ambiente artístico para eventos creativos.",
                    "Perú",
                    "Lima",
                    "Barranco",
                    "Jr. Unión 154",
                    100,
                    40,
                    new List<string> { "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800" },
                    "WiFi,Aire Acondicionado,Terraza,Bar,Iluminación LED",
                    3,
                    1
                ),
                new List<string> { "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800" }
            )
        };

        foreach (var (cmd, photos) in demoLocals)
        {
            var local = new Local(cmd);
            foreach (var photoUrl in photos)
            {
                local.LocalPhotos.Add(new LocalPhoto(photoUrl));
            }
            await localRepository.AddAsync(local);
        }

        await unitOfWork.CompleteAsync();
    }
}
