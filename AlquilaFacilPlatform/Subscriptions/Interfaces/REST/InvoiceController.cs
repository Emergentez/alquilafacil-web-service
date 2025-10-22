using System.Net.Mime;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class InvoiceController(
    IInvoiceCommandService invoiceCommandService,
    IInvoiceQueryService invoiceQueryService) 
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceResource createInvoiceResource)
    {
        var createInvoiceCommand =
            CreateInvoiceCommandFromResourceAssembler.ToCommandFromResource(createInvoiceResource);
        var invoice = await invoiceCommandService.Handle(createInvoiceCommand);
        if (invoice is null) return BadRequest();
        var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
        return Ok(resource);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await invoiceQueryService.Handle(new GetAllInvoicesQuery());
        var resource = invoices.Select(InvoiceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resource);
    }
    
    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> GetInvoiceById(int invoiceId)
    {
        var invoice = await invoiceQueryService.Handle(new GetInvoiceByIdQuery(invoiceId));
        if (invoice is null) return NotFound();
        var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
        return Ok(resource);
    }
}