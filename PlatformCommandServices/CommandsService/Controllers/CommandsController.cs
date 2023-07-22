using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(
            ILogger<CommandsController> logger,
            ICommandRepo repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetAllCommandsForPlatform([FromRoute] int platformId) 
        {
            _logger.LogInformation($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!await _repository.PlaformExitsAsync(platformId)) 
            {
                return NotFound();
            }

            var commands = await _repository.GetCommandsForPlatformAsync(platformId);

            var commandsMapped = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

            return Ok(commandsMapped);
        }
        
        [HttpGet(Name = nameof(GetCommandForPlatform))]
        [Route("{commandId}")]
        public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(
            [FromRoute] int platformId, 
            [FromRoute] int commandId) 
        {
            _logger.LogInformation($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!await _repository.PlaformExitsAsync(platformId))
            {
                return NotFound();
            }

            var command = await _repository.GetCommandAsync(platformId, commandId);

            var commandMapped = _mapper.Map<CommandReadDto>(command);

            return Ok(commandMapped);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateCommandForPlatform(
            [FromRoute] int platformId, 
            [FromBody] CommandCreateDto commandDto) 
        {
            _logger.LogInformation($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!await _repository.PlaformExitsAsync(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId, command);
            await _repository.SaveChangesAsync();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId, commandReadDto.Id }, commandReadDto);
        }

    }
}
