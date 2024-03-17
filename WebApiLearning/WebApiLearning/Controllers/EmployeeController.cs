using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System.Text;
using WebApiLearning.DataAccess.Repository;
using WebApiLearning.Model.IRepository;
using WebApiLearning.Model.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApiLearning.Model.DTO;

namespace WebApiLearning.Controllers
{
    // The first difference between Web API and MVC is The API controller has two important annotations 
    // This controller is decorated with [Route("api/[controller]")] and [ApiController] attributes.

    // [Route("api/[controller]")]: This attribute is used to define the route template for the
    //   controller.
    // In this case, it specifies that routes for this controller will start with "/api/" followed by
    //   the controller name in lowercase.
    // The [controller] token will be replaced with the actual controller name, which in this case is "Employee". 

    // [ApiController]: This attribute is used to indicate that the controller should use certain
    //   conventions to simplify the API development process.
    // It automatically handles model validation, binding source parameter inference, etc.
    // It's a feature introduced in ASP.NET Core 2.1 to streamline the development of APIs.

    [Route("api/[controller]")]
    [ApiController]
    // The second difference is the API controller extends from ControllerBase, on the other hand the
    //   MVC controller extends from Controller.
    // The differences here is the Controller class contains some methods that used for Views like
    //   PartialView, ViewResult, and some Prop like ViewBag, TempData, ViewData. all these properties
    //   are used only with Views.

    // But with the API We don't need to use these properties as there is no Views here.
    // As we extend from ControllerBase we will find that there are some methods that handle status
    //   requests like OK(), Created(), BadRequest(), NotFound().

    public class EmployeeController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // Also we don't have any return as view.


        // Now for Action methods: While we are using Swagger to test API we have to know that, If we
        //   want to specify a method as one of the HTTP methods which are:
        // GET, POST, PUT, DELETE, PATCH and HEAD : Which represent CRUD operations.

        // GET − Provides a read only access to a resource.
        // PUT − Used to update an existing resource or create a new resource.
        // DELETE − Used to remove a resource.
        // POST −Used to create a new resource.

        // To use one of these HTTP methods we have to use it before each method like
        [HttpGet]
        public IActionResult GetEmployees()
        {
            List<Employee> employees = _unitOfWork.Employee.GetAll().ToList();
            return Ok(employees);
        }
        [HttpGet]
        // here we need an Id to get the employee, This Id will be fetched using model Binder, but we
        //   have to know that the model binder with API is difference from model binder with MVC.
        // In ASP.NET MVC, the Model Binder is responsible for mapping HTTP request data (such as form
        //   values, query string parameters, and route data) to action method parameters or action
        //   method parameters to views.
        // It is an essential component in the MVC framework that helps in the automatic conversion of
        //   incoming request data into strongly typed objects.
        // The model binding process occurs before an action method is invoked, allowing action methods
        //   to accept complex object parameters directly.

        // With Web API the model binder searches for primitive data in Rout which will be either (parameter or queryString).
        // If the data is complex the model binder searches in request Body.

        // So As we uses here primitive data (Id) then we need to specify the place that the model
        //   binder will look for.
        // And we have also specify the Route to make the route accept Id
        // So we have to use Parameter Route.
        [Route("/api/[controller]/{id:int}", Name ="EmployeeDetailsRoute")] // This route will override the existing route to add Id to route.
        // Also we could make the route like [Route("{id}")], This also correct, but as we don't start
        //   with / this mean it will not be overridden it will be connected to the existing route
        //   (controller Route).

        // Also we could put Id in HttpGet attribute and use the route and HttPGet in one sentence.
        // [HttpGet("{id}")]

        public IActionResult GetEmployee([FromRoute]/*Here we specify the place that the model binder will search in.*/int id) 
        {
            Employee emp = _unitOfWork.Employee.GetById(id);
            return Ok(emp);
        }
        // Why we put here alpha with name and we put int with id?
        // As we have to end point that makes Swagger confused as He can't figure out which path he has
        //   to go through, The end points are "/api/[controller]/{id}" and "/api/[controller]/{name}"
        // So we have to route which are identical to each other, So the solution is the specify the
        //   type of data that accepted by each route, this is the reason that we uses alpha and int, to
        //   specify the path that must be taken if the data is int or if the data is letters.
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName([FromRoute] string name)
        {
            var emp = _unitOfWork.Employee.Find(e => e.Name == name).ToList();
            return Ok(emp);
        }

        // let's try another verb which is put.
        [HttpPut]
        public IActionResult PutEmployee([FromBody]Employee emp) 
        { 
            if(ModelState.IsValid)
            {
                _unitOfWork.Employee.Update(emp);
                _unitOfWork.Save();
                return StatusCode(StatusCodes.Status204NoContent);
                // return StatusCode(204);
            }
            return BadRequest();
        }
        // here we will try to delete a record form database.
        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            try
            {
               Employee emp =  _unitOfWork.Employee.GetById(id);
                if (emp != null)
                {
                    _unitOfWork.Employee.Delete(emp);
                    _unitOfWork.Save();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]Employee emp)
        {
            // first we have to check if the model state is valid or not.
            if (ModelState.IsValid)
            {
                _unitOfWork.Employee.Add(emp); 
                _unitOfWork.Save();
                // now we will not return OK, but we will return Created method 
                // The created method has three overloads 
                // 1. doesn't accept anything.
                // 2. Accepts String Url and Object Value. 
                //      The URI at which the content has been created.
                //      The content value to format in the entity body.
                // 3. Accepts Url url and Object value.
                // we will use the second overload here which accepts Url as string and object that has been
                //   added to data base.
                // So we have to create the url that will be passed to this method, so we have to use the Url
                // class which contains Link method that Accepts Route Name, and Object value.
                // But because we don't have a Rout Name so we have to back again to the HttpGet method and
                //  add there name for its route.[HttpGet("{id:int}", Name ="EmployeeDetailsRoute")]
                string url = Url.Link("EmployeeDetailsRoute", new { id = emp.Id }/*This is the id for the new employee.*/);
                return Created(url, emp);
            }
            return BadRequest(ModelState);
        }

        // Now after we create the department table and make a relation between Department and Employee table
        //   when we try to get employee we will find that the Department prop in Employee will be null as
        //   the dot net doesn't all lazy loading so it get only the primitive type not the complex type.
        /* {
            "id": 3,
            "name": "Emad",
            "address": "Alx",
            "salary": 7000,
            "departmentId": 1,
            "department": null
           }
         */
        // To Solve this problem then we have to make the dot net allow lazy loading.
        // This could happen using Include.
        [HttpGet("dep/{id:int}")]
        public IActionResult GetEmployeeWithDep(int id)
        {
            // In the following query we need to retrieve the employee from the database with its
            //   department data, but when i try to do this using Include method, It will give me the
            //   following error.
            /*
             * System.Text.Json.JsonException: A possible object cycle was detected. This can either be due
             * to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider 
             * using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles. Path:
             * $.Department.Employees.Department.Employees.Department.Employees.Department.Employees.Depart
             * ment.Employees.Department.Employees.Department.Employees.Department.Employees.Department.Emp
             * loyees.Department.Employees.Department.Id.
             *    at System.Text.Json.ThrowHelper.ThrowJsonException_SerializerCycleDetected(Int32 maxDepth)
             */

            Employee emp = _unitOfWork.Employee.Include(e => e.Department, e => e.Id == id);
            return Ok(emp);
            // The previous error mean that there is a cyclic dependency between Employee and Department
            //   table which means when trying to get data form employee it lead to department which leads
            //   to employee which leads to department and so on, this repeating will get infinite cycle,
            //   So it returns the previous error.

            // The best solution for this case is to use DTO (Data Traverse Object).

            // I will try to do this with new action

        }
        [HttpGet("DTO/{id:int}")]
        public IActionResult GetEmployeeWithDepDto(int id)
        {
            try
            {
                Employee emp = _unitOfWork.Employee.Include(e => e.Department, e => e.Id == id);
                // Here we create an instance of EmployeeNameWithDepartmentNameDTO, then we will add data for
                //   this object which i already get from emp with IncludeDepartment.
                if (emp != null)
                {
                    EmployeeNameWithDepartmentNameDTO empDep = new EmployeeNameWithDepartmentNameDTO();
                    empDep.EmployeeName = emp.Name;
                    empDep.DepartmentName = emp.Department.Name;
                    return Ok(empDep);
                }
                else
                {
                    return NotFound("There is no employee with this Id.");
                }
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
