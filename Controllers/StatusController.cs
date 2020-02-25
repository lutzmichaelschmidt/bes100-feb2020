using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : Controller
    {
        //public StatusController(IGenerateEmployeeIds)
        //{

        //}

        [HttpGet("status")]
        public ActionResult<StatusResponse> GetStatus()
        {
            var response = new StatusResponse
            {
                Status = "OK",
                CreatedAt = DateTime.Now
            };
            return Ok(response);
        }

        // Resource Archetypes
        // 1. Collection (usually plural, a set of things) /Employees
        // 2. Document (Singular. A single thingy)
        // 3. Store
        // 4. Controller

        // Getting information INTO the API
        // 1. Url
        //    a. Just plain ole' routing.
        //    b. Embed some variables in the route itself. e.g. GET /employees/657/salary

        [HttpGet("employees/{employeeId:int}/salary")]
        public ActionResult GetEmployeeSalary(int employeeId)
        {
            return Ok($"Employee {employeeId} makes 60K");
        }
        //    c. Add some data to the query string at the end of the URL e.g. GET /shoes?color=blue \
        [HttpGet("shoes")]
        public ActionResult GetShoes([FromQuery]string color = "ALL")
        {
            return Ok($"Getting you shoes of color {color}");
        }

        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name = "User-Agent")]string userAgent)
        {
            return Ok($"You are {userAgent}");
        }

        [HttpGet("employees")]
        public ActionResult GetEmployees()
        {
            return Ok($"All Employees");
        }

        [HttpPost("employees")]
        public ActionResult AddEmployee([FromBody] NewEmployee employee, [FromServices] IGenerateEmployeeIds idGenerator)
        {
            //var idGenerator = new EmployeeIdGenerator();

            employee.Id = idGenerator.GetNewEmployeeId();
            return Ok($"Hiring {employee.Name} starting at {employee.StartingSalary.ToString("c")} ID:{employee.Id}");
        }
    }




    public class NewEmployee
    {
        public string Name { get; set; }
        public decimal StartingSalary { get; set; }
        public Guid Id { get; set; }
    }




    public class StatusResponse
    {
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
