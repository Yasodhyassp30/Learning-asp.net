
using MongoDB.Driver;
using AllInOne.Models;

namespace AllInOne.Services;

    public class UserService
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly ILogger<UserService> _logger;
        public UserService(MongoDb database,ILogger<UserService> logger)
        {
            _logger= logger;
            try{
                _users = database.Users;
            }catch(Exception ex){
                _logger.LogError(ex.Message);
            }
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _users.Find(user => true).ToListAsync();
        }
        public async Task<UserModel> GetUserByID(string id)
        {
            return await _users.Find<UserModel>(user => user.Id == id).FirstOrDefaultAsync();
        }
        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _users.Find<UserModel>(user => user.Email == email).FirstOrDefaultAsync();
        }
        public async Task<UserModel> CreateUser(UserModel user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }



        
    }

