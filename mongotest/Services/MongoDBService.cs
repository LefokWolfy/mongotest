//using MongoDB.Driver;
//using MongoAPI.Models;
//using Microsoft.Extensions.Configuration;

//namespace MongoAPI.Services
//{
//    public class MongoDBService
//    {
//        private readonly IMongoCollection<Customer> _customersCollection;

//        public MongoDBService(IConfiguration config)
//        {
//            var client = new MongoClient(config.GetConnectionString("MongoDB"));
//            var database = client.GetDatabase("test"); // Change to your database name
//            _customersCollection = database.GetCollection<Customer>("Customers"); // Collection name
//        }

//        public async Task AddCustomerAsync(Customer customer) =>
//            await _customersCollection.InsertOneAsync(customer);
        
//        public async Task<IEnumerable<Customer>> GetCustomersAsync() =>
//            await _customersCollection.Find(customer => true).ToListAsync();

//        public async Task<Customer> GetCustomerAsync(string id) =>
//            await _customersCollection.Find<Customer>(customer => customer.Id == id).FirstOrDefaultAsync();

//        public async Task UpdateCustomerAsync(string id, Customer customerIn) =>
//            await _customersCollection.ReplaceOneAsync(customer => customer.Id == id, customerIn);
        
//        public async Task RemoveCustomerAsync(string id) =>
//            await _customersCollection.DeleteOneAsync(customer => customer.Id == id);
//    }
//}
