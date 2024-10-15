using DemoCacheing.Database;
using Microsoft.Extensions.Caching.Memory;

namespace DemoCacheing.Services;

public class EmployeeService(EmployeeRepository repository, IMemoryCache cache)
{
    private readonly EmployeeRepository _repository = repository;
    private readonly IMemoryCache _cache = cache;

    private const string EmployeeCacheKey = "EmployeeList";
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public async Task<Employee> CreateEmployeeAsync(Employee request)
    {
        await _repository.CreateEmployeeAsync(request);
        _cache.Remove(EmployeeCacheKey);

        return request;
    }
    public async Task<IEnumerable<Employee>?> GetEmployeesAsync()
    {
        if (_cache.TryGetValue(EmployeeCacheKey, out IEnumerable<Employee>? employees))
        {
            Console.WriteLine("employees found in cache");
            return employees;
        }

        await semaphore.WaitAsync();
        try
        {
            if (_cache.TryGetValue(EmployeeCacheKey, out employees))
            {
                Console.WriteLine("employees found in cache");
                return employees;
            }

            employees = await _repository.GetEmployeesAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1);

            _cache.Set(EmployeeCacheKey, employees, cacheEntryOptions);
        }
        finally
        {
            semaphore.Release();
        }

        return employees;
    }

    public async Task<List<Employee>?> GetEmployeesNoCacheAsync()
    {
        var employees = await _repository.GetEmployeesAsync();
        return employees;
    }
}
