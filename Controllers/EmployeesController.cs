using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    // localhost:xxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var allEmployees = await dbContext.Employees.ToListAsync();

                return Ok(allEmployees);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occured while fetching employees: {ex.Message}");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto addEmployeeDto)
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

                return Ok(employeeEntity);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error Creatind Employee: {ex.Message}");
            }
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await dbContext.Employees.FindAsync(id);
                if (employee is null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occured while trying to retrieve an Employee: {ex.Message}");
            }
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id,UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await dbContext.Employees.FindAsync(id);
                if (employee is null)
                {
                    return NotFound();
                }

                employee.Email = updateEmployeeDto.Email;
                employee.Name = updateEmployeeDto.Name;
                employee.Phone = updateEmployeeDto.Phone;
                employee.Salary = updateEmployeeDto.Salary;

                await dbContext.SaveChangesAsync();
                return Ok(employee);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An Error occured updating Employee data: {ex.Message}");
            }

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeById(Guid id) 
        {
            try
            {
                var employee = await dbContext.Employees.FindAsync(id);

                if (employee is null)
                {
                    return NotFound();
                }

                dbContext.Employees.Remove(employee);
                await dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occured while trying to delete employee: {ex.Message}");
            }
           

        }
    }
}
