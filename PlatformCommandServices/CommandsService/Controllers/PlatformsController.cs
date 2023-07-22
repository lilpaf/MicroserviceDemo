using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(
            ILogger<PlatformsController> logger,
            ICommandRepo repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms() 
        {
            _logger.LogInformation("--> Getting platforms from Commands Service");

            var platformItems = await _repository.GetAllPlatformsAsync();

            var platformItemsMapped = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);

            return Ok(platformItemsMapped);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            _logger.LogInformation("--> Inbound POST # Commands Service");

            return Ok("Inbound test for Platforms Controller");
        }
    }
}
