using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface IInvoiceQueryService
{
    Task<Invoice?> Handle(GetInvoiceByIdQuery query);
    Task<IEnumerable<Invoice>> Handle(GetAllInvoicesQuery query);
}