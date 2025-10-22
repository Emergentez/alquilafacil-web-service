using AlquilaFacilPlatform.Subscriptions.Domain.Model.Queries;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPlans()
    {
        var getAllPlansQuery = new GetAllPlansQuery();
        var plans = await planQueryService.Handle(getAllPlansQuery);
        var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

}