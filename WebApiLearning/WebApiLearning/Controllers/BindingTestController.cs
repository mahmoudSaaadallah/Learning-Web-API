using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLearning.Model.Models;

namespace WebApiLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindingTestController : ControllerBase
    {
        // When we create Action method without specifying the type of HTTP Response for this action method 
        //   the swagger can't reach for this action method, but the postMan will be able to detect it if the
        //   name convention contains type of http response like get, post, put, delete.
        [HttpGet("{id:int}/{Name:alpha}")]// This to define type of http response.
        // When we dealing with API we have to know how the binding works with it.
        // If the Action method accepts an primitive type then the binding will search about this data in 
        //   1. Route  or   2. Query string 
        // And this happens by default.
        // So in the following method If it Accepts an id and string Name, and we didn't specify where the
        //   binding will search about this data then the binding will search Query string.

        // But if I specify where the binding will search for this data like when I specify to search in
        //   Route by adding [HttpGet("{id:int}/{Name:alpha}")] before the method this mean the binding
        //   will search for in route. 

        // Without specifying where the binding will search it will search in query string.

        public IActionResult get(int id, string Name)// Here we didn't specify where the binding will search 
                                                     //   for Id and Name, so it will search by default in
                                                     //   Route and Query string
        {
            return Ok();
        }


        // Now we the complex objects:
        // If the method accepts a complex object like the post method then the binding will search by
        //   default in body.
        [HttpPost]
        // Here in the following method which accepts a complex object of type employee, and we didn't
        //   specify where the binding will search for emp then it will search by default in body.
        public IActionResult post(Employee emp)
        {
            return Ok(emp);
        }

        // What about if i make a get function that accepts a complex object of type employee, As We know
        //   the get function return data not accepting it from body, because it has no body.
        // So to solve this problem we have to specify the place of data like queryString
        [HttpGet]
        public IActionResult get([FromQuery]Employee emp/*Here we specify the place of data that will be accepted using Get*/)
        {
            return Ok(emp);
        }
        // I could make the same method, but this time i will accept the complex data from Rout not from
        //   Query string.
        // As we will accept data from route then we have to make HTTPGet Attribute know that the data will
        //   be accepted from route.
        [HttpGet("{id:int}/{name:alpha}/{address:alpha}/{salary:decimal}")]
        public IActionResult get2([FromRoute]Employee emp)
        {
            return Ok(emp);
        }

        // As we know that the string name accepts from route or query string, but what If I want to make
        //   the string accepted from body then we have to specify this case.
        // Also we have to know that the body accepts just one data, so I can't send two complex string in
        //   the same body or sending two data in the same body.
        [HttpPost("body") /*We changed here url because we have anther post method with the same end point, So we have to change end point for this one*/]
        public IActionResult post2([FromBody] string name)
        {
            return Ok(name);
        }
    }
}
