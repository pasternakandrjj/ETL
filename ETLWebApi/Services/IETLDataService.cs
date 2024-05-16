using ETLWebApi.Models;

namespace ETLWebApi.Services
{
    public interface IETLDataService
    {
        Task ImportData();

        Task<int> FindPULocationId();

        Task<IEnumerable<ETLData>> FindLongestTripDistance();

        Task<IEnumerable<ETLData>> FindLongestTimeSpentTravelling();

        Task<IEnumerable<ETLData>> GetDatas(int pULocationId);
    }
}