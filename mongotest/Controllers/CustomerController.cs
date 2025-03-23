//using Microsoft.AspNetCore.Mvc;
//using MongoAPI.Models;
//using MongoAPI.Services;

//namespace MongoAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomerController : ControllerBase
//    {
//        private readonly MongoDBService _mongoDBService;

//        public CustomerController(MongoDBService mongoDBService)
//        {
//            _mongoDBService = mongoDBService;
//        }

//        // POST: api/customer
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] CustomerDTO customerDTO)
//        {
//            var customer = new Customer
//            {
//                Name = customerDTO.Name,
//                Age = customerDTO.Age,
//                Email = customerDTO.Email
//            };

//            await _mongoDBService.AddCustomerAsync(customer);
//            return CreatedAtAction(nameof(Post), new { id = customer.Id }, customer);
//        }

//        // GET: api/customer
//        [HttpGet]
//        public async Task<IActionResult> Get() =>
//            Ok(await _mongoDBService.GetCustomersAsync());

//        // GET: api/customer/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(string id)
//        {
//            var customer = await _mongoDBService.GetCustomerAsync(id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            return Ok(customer);
//        }

//        // PUT: api/customer/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(string id, [FromBody] CustomerDTO customerDTO)
//        {
//            var customer = await _mongoDBService.GetCustomerAsync(id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            customer.Name = customerDTO.Name;
//            customer.Age = customerDTO.Age;
//            customer.Email = customerDTO.Email;

//            await _mongoDBService.UpdateCustomerAsync(id, customer);
//            return Ok(customer);
//        }

//        // DELETE: api/customer/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var customer = await _mongoDBService.GetCustomerAsync(id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            await _mongoDBService.RemoveCustomerAsync(id);
//            return NoContent();
//        }
//    }
//}
