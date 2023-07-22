using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private const string PlatformPublishedEvent = "Platform_Published";
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ProcessEventAsync(string message)
        {
            var eventType = DetermineEvent(message);

            if (eventType == EventType.PlatformPublished)
            {
                await AddPlatformAsync(message);
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("--> Determining event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case PlatformPublishedEvent:
                    _logger.LogInformation("--> Platform Published event detected");
                    return EventType.PlatformPublished;

                default:
                    _logger.LogError("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task AddPlatformAsync(string platformPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);

                    if (!await repo.ExternalPlatformExistsAsync(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        await repo.SaveChangesAsync();

                        return;
                    }

                    _logger.LogError("--> Platform already exisits...");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Could not add Platform to DB: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
