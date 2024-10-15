using DemoCacheing.Database;
using Microsoft.EntityFrameworkCore;

namespace DemoCacheing.Services;

public class EmployeeRepository(MainContext context)
{
    private readonly MainContext _context = context;

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees.ToListAsync();
    }
}
