using EmployeeAdminPortal.Models.Entities;
using EmployeeAdminPortal.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeAdminPortal.Data;

namespace EmployeeAdminPortal.Services
{
    public class EmployeeServices
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Object> AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            try
            {
                var employeeEntity = new Employee()
                {
                    Email = addEmployeeDto.Email,
                    Name = addEmployeeDto.Name,
                    Phone = addEmployeeDto.Phone,
                    Salary = addEmployeeDto.Salary
                };

                await dbContext.Employees.AddAsync(employeeEntity);
                await dbContext.SaveChangesAsync();

                return employeeEntity;
            }
            catch (Exception ex)
            {

                return new { Error = $"Error Creatind Employee: {ex.Message}"};
            }
        }

        public async Task<Object> GetEmployees()
        {
            try
            {
                var employees = await dbContext.Employees.ToListAsync();
                return employees;
            }
            catch (Exception ex)
            {

                return ($"an error occured retrieving employees: {ex.Message}");
            }
            
        }

        public async Task<Object> UpdateEmployeeData(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await dbContext.Employees.FindAsync(id);
                if(employee is null)
                {
                    return ("employee not found");
                }

                employee.Email = updateEmployeeDto.Email;
                employee.Name = updateEmployeeDto.Name;
                employee.Phone = updateEmployeeDto.Phone;
                employee.Salary = updateEmployeeDto.Salary;

                await dbContext.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {

                return new { Error = $"An error occured while updating employee data: {ex.Message}" };
            }
        }

        public async Task<Object> GetEmployeeById(Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return ("employee not found");
            }

            return employee;
        }

        public async Task DeleteEmployee(Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return;
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
        }

    }
}
