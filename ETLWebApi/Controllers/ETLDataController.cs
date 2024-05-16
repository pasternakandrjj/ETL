using ETLWebApi.Models;
using ETLWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETLWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ETLDataController : ControllerBase
    {
        private readonly ILogger<ETLDataController> _logger;

        private readonly IETLDataService _iETLDataService;

        public ETLDataController(ILogger<ETLDataController> logger, IETLDataService iETLDataService)
        {
            _logger = logger;
            _iETLDataService = iETLDataService;
        }

        //7129 rows being written
        [HttpGet("ImportData")]
        public async Task<IActionResult> ImportDataAsync()
        {
            try
            {
                await _iETLDataService.ImportDataAsync();
                return Ok("Data imported successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error importing data: " + ex.Message);
            }
        }

        [HttpGet("LocationWithHighestTipAmount")]
        public async Task<int> FindLocationWithHighestTipAmountAsync()
        {
            return await _iETLDataService.FindPULocationIdAsync();
        }

        [HttpGet("LongestTripsByDistance")]
        public async Task<IEnumerable<ETLData>> FindLongestTripDistanceAsync()
        {
            return await _iETLDataService.FindLongestTripDistanceAsync();
        }

        [HttpGet("LongestTripsByTimeSpent")]
        public IEnumerable<ETLData> FindLongestTimeSpentTravelling()
        {
            return _iETLDataService.FindLongestTimeSpentTravelling();
        }

        [HttpGet("SearchByPULocationId")]
        public async Task<IEnumerable<ETLData>> GetDatasFilteredByPULocationIdAsync(int pULocationId)
        {
            return await _iETLDataService.GetDatasAsync(pULocationId);
        }
    }
}