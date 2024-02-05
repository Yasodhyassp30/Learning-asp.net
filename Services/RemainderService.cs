

using AllInOne.Models;
using MongoDB.Driver;

namespace AllInOne.Services;

public class RemainderService{
    private readonly IMongoCollection<RemainderModel> _remainders;
    private readonly ILogger<RemainderService> _logger;

    public RemainderService(MongoDb database,ILogger<RemainderService> logger)
    {
        _logger= logger;
        try{
            _remainders = database.Remainders;
        }catch(Exception ex){
            _logger.LogError(ex.Message);
        }
    }

    public async Task<List<RemainderModel>> GetRemainders(string userId)
    {
        return await _remainders.Find(remainder => remainder.UserId == userId).ToListAsync();
    }

    public async Task<RemainderModel> GetRemainderByID(string id)
    {
        return await _remainders.Find<RemainderModel>(remainder => remainder.Id == id).FirstOrDefaultAsync();
    }
}