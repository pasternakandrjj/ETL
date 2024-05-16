using ETLWebApi.Models;

namespace ETLWebApi.Services
{
    public interface IETLDataService
    {
        Task ImportDataAsync();

        Task<int> FindPULocationIdAsync();

        Task<IEnumerable<ETLData>> FindLongestTripDistanceAsync();

        IEnumerable<ETLData> FindLongestTimeSpentTravelling();

        Task<IEnumerable<ETLData>> GetDatasAsync(int pULocationId);
    }
}