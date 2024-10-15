using DemoCaching.Database;
using DemoCaching.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DemoCaching.Controllers;

[Route("api/")]
[ApiController]
public class EmployeeController(EmployeeService service) : ControllerBase
{
    private readonly EmployeeService _service = service;

    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee(Employee request)
    {
        var employee = await _service.CreateEmployeeAsync(request);
        return Ok(employee);
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployeesWithCache()
    {
       var employees = await _service.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("employees1")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _service.GetEmployeesNoCacheAsync();
        return Ok(employees);
    }
}
