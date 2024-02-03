
using MongoDB.Driver;
using MongoDB.Bson;
using AllInOne.Models;
using Microsoft.Extensions.Options;

namespace AllInOne.Services;

    public class UserService
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly ILogger<UserService> _logger;
        public UserService(IOptions<MongoDBSettings> settings, ILogger<UserService> logger)
        {
            _logger= logger;
            try{
            _logger.LogInformation("Connecting to MongoDB");
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<UserModel>(settings.Value.CollectionName);
            }catch(Exception ex){
                _logger.LogError(ex.Message);
            }
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _users.Find(user => true).ToListAsync();
        }
        public async Task<UserModel> GetUser(string id)
        {
            return await _users.Find<UserModel>(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            Console.WriteLine(user);
            await _users.InsertOneAsync(user);
            return user;
        }
    }

