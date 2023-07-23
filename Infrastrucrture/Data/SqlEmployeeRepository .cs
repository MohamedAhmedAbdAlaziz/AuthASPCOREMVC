using Core.Models;

namespace Infrastrucrture.Data
{
    public class SqlEmployeeRepository  : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public SqlEmployeeRepository(AppDbContext appContext)
        {
            _context = appContext;
        }

        public Employee AddEmployee(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee e= _context.Employees.Find(id);
           if(e!= null) {
            _context.Employees.Remove(e);
            _context.SaveChanges();
        }
         
            return e;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
           return _context.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            Employee e= _context.Employees.Find(Id);
            return e;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var e= _context.Employees.Attach(employee);
            e.State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employee;
        }
    }
}