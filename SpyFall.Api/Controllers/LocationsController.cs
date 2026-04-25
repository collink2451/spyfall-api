using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;
using SpyFall.Api.DTOs;

namespace SpyFall.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext mDb = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<LocationResponse> locations = await mDb.Locations
            .OrderBy(l => l.Name)
            .Select(l => new LocationResponse { Id = l.Id, Name = l.Name })
            .ToListAsync();

        return Ok(locations);
    }
}
