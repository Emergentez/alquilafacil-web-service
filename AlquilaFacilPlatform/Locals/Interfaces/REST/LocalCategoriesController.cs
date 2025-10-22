using System.Net.Mime;
using AlquilaFacilPlatform.Locals.Domain.Model.Queries;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AlquilaFacilPlatform.Locals.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
        var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
        var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localCategoryResources);
    }
}
