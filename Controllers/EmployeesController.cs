using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using EmployeeAdminPortal.Services;
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
        private readonly EmployeeServices _employeeService;

        public EmployeesController(ApplicationDbContext dbContext, EmployeeServices employeeService)
        {
            this.dbContext = dbContext;
            this._employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var allEmployees = await _employeeService.GetEmployees();

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
                var addedEmployee = await _employeeService.AddEmployee(addEmployeeDto);
                return Ok(addedEmployee);
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
                var employee = await _employeeService.UpdateEmployeeData(id, updateEmployeeDto);
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
                var employee = _employeeService.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occured while trying to delete employee: {ex.Message}");
            }
           

        }
    }
}
