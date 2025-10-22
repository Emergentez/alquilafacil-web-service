using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Entities;

namespace AlquilaFacilPlatform.Subscriptions.Domain.Services;

public interface IInvoiceCommandService
{
    public Task<Invoice?> Handle(CreateInvoiceCommand command);
}