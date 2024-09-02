using JwtAuthentication.DbContextHelper;
using JwtAuthentication.Model;
using JwtAuthentication.Services.Interface;

namespace JwtAuthentication.Services.Implementation
{
    /// <summary>
    /// EmployeeService
    /// </summary>
    public class EmployeeService : IEmployee
    {
        private readonly ApplicationContext _appDbContextService;

        public EmployeeService(ApplicationContext applicationContext) 
        {
            _appDbContextService = applicationContext;
        }
        /// <summary>
        /// GetEmployeeByEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<Employee> GetEmployeeByEmail(string email)
        {
            var employee = new Employee();
            try
            {
               employee =  _appDbContextService.Employees.Where(x => x.Email == email).FirstOrDefault();
            }
            catch (Exception ex) {
                
            }
            return Task.FromResult(employee);
        }

        /// <summary>
        /// GetEmployees
        /// </summary>
        /// <returns></returns>
        public Task<List<Employee>> GetEmployees()
        {
            var employees = new List<Employee>();
            try
            {
                employees = _appDbContextService.Employees.ToList();
            }
            catch (Exception ex) { }
            return Task.FromResult(employees);
        }
    }
}
