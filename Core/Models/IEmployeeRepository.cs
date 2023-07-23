using System.Collections.Generic;

namespace Core.Models
{
    public interface IEmployeeRepository
    {
         Employee GetEmployee(int Id);
        IEnumerable<Employee> GetAllEmployees();
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        Employee Delete(int id);
    }
}
