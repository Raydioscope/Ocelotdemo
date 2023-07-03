using Authentication.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Authentication.Services;

public class UserService
{
    private readonly IMongoCollection<Users> _UserCollection;

    public UserService(
        IOptions<ELMSDatabaseSettings> UsersDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            UsersDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            UsersDatabaseSettings.Value.DatabaseName);

        _UserCollection = mongoDatabase.GetCollection<Users>(
            UsersDatabaseSettings.Value.UserCollectionName);
    }

        public async Task<List<Users>> GetAsync() =>
            await _UserCollection.Find(_ => true).ToListAsync();

        public async Task<Users?> GetAsync(string id , string pass) =>
            await _UserCollection.Find(x =>  x.UserID == id && x.Password == pass ).FirstOrDefaultAsync();

        public async Task CreateAsync(Users newUser) =>
            await _UserCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, Users updatedUser) =>
            await _UserCollection.ReplaceOneAsync(x => x.UserID == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _UserCollection.DeleteOneAsync(x => x.UserID == id);

        public async Task<List<Users>> GetByRoleAsync(string role) =>
            await _UserCollection.Find(x => x.Role == role).ToListAsync();
}