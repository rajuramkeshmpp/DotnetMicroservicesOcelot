using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using SuperAdminService.Models;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly IDynamoDBContext _dbContext;

    public UserRoleController(IDynamoDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] UserRole userRole)
    {
        var allUserRoles = await _dbContext.ScanAsync<UserRole>(new List<ScanCondition>()).GetRemainingAsync();
        int nextId = allUserRoles.Any() ? allUserRoles.OrderByDescending(ur => ur.Id).First().Id + 1 : 1;

        userRole.Id = nextId;
        await _dbContext.SaveAsync(userRole);

        return Ok(userRole);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllUserRoles()
    {
        var userRoles = await _dbContext.ScanAsync<UserRole>(new List<ScanCondition>()).GetRemainingAsync();
        return Ok(userRoles);
    }
}
