using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{

    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees;
        public MockEmployeeRepository()
        {
            _employees = new List<Employee>()
            {
                new Employee()
                { Id = 1, Name = "Mohamed", Department = Dept.IT, Email = "Mary@gimal.com" },
                new Employee() { Id = 2, Name = "Mayar", Department = Dept.HR, Email = "Mayar@gimal.com" },
                new Employee() { Id = 3, Name = "Gehad", Department = Dept.Payroll, Email = "Gehad@gimal.com" },
            };

        }

        public Employee AddEmployee(Employee employee)
        {
        int tt=  _employees.Max(e=>e.Id);
        employee.Id=tt+1;
         _employees.Add(employee);
         
          return employee ;
 
        }

        public Employee Delete(int id)
        {
            Employee employee= _employees.FirstOrDefault(x => x.Id == id);
            if(employee != null){
                _employees.Remove(employee);
            }

            return employee;

        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;  
        }

        public Employee GetEmployee(int Id)
        {
            return _employees.FirstOrDefault(x => x.Id == Id);
        }

        public Employee UpdateEmployee(Employee employee)
        {
            Employee emplo= _employees.FirstOrDefault(x => x.Id == employee.Id);
            if(emplo!= null){
                emplo.Name= employee.Name;
                emplo.Email= employee.Email;
                emplo.Department= employee.Department;
            };
            return emplo;
        }
    }
}
