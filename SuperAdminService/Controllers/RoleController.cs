using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using SuperAdminService.Models;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IDynamoDBContext _dbContext;

    public RoleController(IDynamoDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRole([FromBody] Role role)
    {
        var allRoles = await _dbContext.ScanAsync<Role>(new List<ScanCondition>()).GetRemainingAsync();
        int nextId = allRoles.Any() ? allRoles.OrderByDescending(r => r.Id).First().Id + 1 : 1;

        role.Id = nextId;
        await _dbContext.SaveAsync(role);

        return Ok(role);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _dbContext.ScanAsync<Role>(new List<ScanCondition>()).GetRemainingAsync();
        return Ok(roles);
    }
}
