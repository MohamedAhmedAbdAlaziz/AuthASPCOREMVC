using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucrture.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder){
            modelBuilder.Entity<Employee>().HasData(
                  new Employee()
                { Id = 1, Name = "Mohamed", Department = Dept.IT, Email = "Mary@gimal.com" },
                new Employee() { Id = 2, Name = "Mayar", Department = Dept.HR, Email = "Mayar@gimal.com" },
                new Employee() { Id = 3, Name = "Gehad", Department = Dept.Payroll, Email = "Gehad@gimal.com" }
            );
        }
    }
}