using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;

namespace AlquilaFacilPlatform.Subscriptions.Application.Internal.QueryServices;

public class InvoiceQueryService(IInvoiceRepository invoiceRepository) : IInvoiceQueryService
{
    public async Task<Invoice?> Handle(GetInvoiceByIdQuery query)
    {
        return await invoiceRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Invoice>> Handle(GetAllInvoicesQuery query)
    {
        return await invoiceRepository.ListAsync();
    }
}