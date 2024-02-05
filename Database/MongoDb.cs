using MongoDB.Driver;
using AllInOne.Models;
using Microsoft.Extensions.Options;

namespace AllInOne.Services;

public class MongoDb{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDb> _logger;
    public MongoDb(IOptions<MongoDBSettings> settings,ILogger<MongoDb> logger)
    {
        _logger= logger;
        try{
        _logger.LogInformation("Connecting to MongoDB");
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);

        }catch(Exception ex){
            _logger.LogError("Failed to connect to MongoDB");
            _logger.LogError(ex.Message);
        }
    }

    public IMongoCollection<UserModel> Users => _database.GetCollection<UserModel>("Users");
    public IMongoCollection<RemainderModel> Remainders => _database.GetCollection<RemainderModel>("Remainders");

}

