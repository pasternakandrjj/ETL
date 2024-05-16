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

        [HttpGet("ImportData")]
        public void ImportData()
        {
            _iETLDataService.ImportData();
        }

        [HttpGet("FindLocationWithHighestTipAmount")]
        public async Task<int> FindLocationWithHighestTipAmountAsync()
        {
            return await _iETLDataService.FindPULocationId();
        }

        //[HttpGet("GenerateRectangles")]
        //public async Task<IEnumerable<Rectangle>> GenerateRectanglesAsync()
        //{
        //    return await _iETLDataService.ImportData();
        //}
    }
}