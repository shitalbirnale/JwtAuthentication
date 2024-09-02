using JwtAuthentication.Model;

namespace JwtAuthentication.Services.Interface
{
    public interface IEmployee
    {
        Task<Employee> GetEmployeeByEmail(string email);
        Task<List<Employee>> GetEmployees();
    }
}
