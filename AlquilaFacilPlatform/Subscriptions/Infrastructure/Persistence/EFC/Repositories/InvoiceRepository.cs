using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;

namespace AlquilaFacilPlatform.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class InvoiceRepository(AppDbContext context) : BaseRepository<Invoice>(context), IInvoiceRepository
{
    
}