using System.Net.Mime;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST;

/// <summary>
/// Controller for managing subscription invoices
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for invoice management")]
public class InvoiceController(
    IInvoiceCommandService invoiceCommandService,
    IInvoiceQueryService invoiceQueryService)
    : ControllerBase
{
    /// <summary>
    /// Create a new invoice
    /// </summary>
    /// <param name="createInvoiceResource">Invoice creation data</param>
    /// <returns>The created invoice</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new invoice", Description = "Creates a new invoice for a subscription payment")]
    [SwaggerResponse(200, "Invoice created successfully")]
    [SwaggerResponse(400, "Invalid invoice data")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceResource createInvoiceResource)
    {
        var createInvoiceCommand =
            CreateInvoiceCommandFromResourceAssembler.ToCommandFromResource(createInvoiceResource);
        var invoice = await invoiceCommandService.Handle(createInvoiceCommand);
        if (invoice is null) return BadRequest();
        var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
        return Ok(resource);
    }

    /// <summary>
    /// Get all invoices
    /// </summary>
    /// <returns>List of all invoices</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all invoices", Description = "Retrieves a list of all invoices in the system")]
    [SwaggerResponse(200, "List of invoices retrieved successfully")]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await invoiceQueryService.Handle(new GetAllInvoicesQuery());
        var resource = invoices.Select(InvoiceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resource);
    }

    /// <summary>
    /// Get invoice by ID
    /// </summary>
    /// <param name="invoiceId">The invoice ID</param>
    /// <returns>The invoice details</returns>
    [HttpGet("{invoiceId}")]
    [SwaggerOperation(Summary = "Get invoice by ID", Description = "Retrieves a specific invoice by its ID")]
    [SwaggerResponse(200, "Invoice retrieved successfully")]
    [SwaggerResponse(404, "Invoice not found")]
    public async Task<IActionResult> GetInvoiceById(int invoiceId)
    {
        var invoice = await invoiceQueryService.Handle(new GetInvoiceByIdQuery(invoiceId));
        if (invoice is null) return NotFound();
        var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
        return Ok(resource);
    }
}