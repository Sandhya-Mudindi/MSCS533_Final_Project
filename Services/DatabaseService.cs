using LocationTracking.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace LocationTracking.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        //  Event to notify when a new location is saved
        public event Action<LocationData> LocationSaved;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LocationData>().Wait();
        }

        public async Task<int> SaveLocationAsync(LocationData location)
        {
            var result = await _database.InsertAsync(location);

            LocationSaved?.Invoke(location);

            return result;
        }

        public Task<List<LocationData>> GetLocationsAsync() => _database.Table<LocationData>().ToListAsync();
        public async Task<LocationData> GetLastSavedLocationAsync()
        {
            return await _database.Table<LocationData>()
                                  .OrderByDescending(l => l.Timestamp)
                                  .FirstOrDefaultAsync();
        }
    }
}
