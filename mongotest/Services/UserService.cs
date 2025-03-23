using MongoDB.Driver;
using MongoAPI.Models;
using Microsoft.Extensions.Configuration;
using mongoAPI.Models;
using MongoDB.Bson;

namespace mongoAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("forum"); // Change to your database name
            _usersCollection = database.GetCollection<User>("Users"); // Collection name
        }

        public async Task AddCustomerAsync(User user) =>
            await _usersCollection.InsertOneAsync(user);

        public async Task<IEnumerable<User>> GetCustomersAsync() =>
            await _usersCollection.Find(user => true).ToListAsync();

        public async Task<User> GetCustomerAsync(ObjectId id) =>
            await _usersCollection.Find<User>(user => user.UserId == id).FirstOrDefaultAsync();

        public async Task UpdateCustomerAsync(ObjectId id, User user) =>
            await _usersCollection.ReplaceOneAsync(user => user.UserId == id, user);

        public async Task RemoveCustomerAsync(ObjectId id) =>
            await _usersCollection.DeleteOneAsync(user => user.UserId == id);
    }
}
