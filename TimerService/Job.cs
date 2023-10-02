using System.Text.Json;
using MessageService.In;
using Quartz;

namespace TimerService;

public class Job : IJob
{
    private readonly IMessageService _messageService;

    public Job(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Execute(IJobExecutionContext context)
    {

        var message = new GameMessage()
        {
            Id = Ulid.NewUlid(),
            Message = "New Game",
            Created = context.FireTimeUtc.DateTime
        };
        await _messageService.SendAsync(JsonSerializer.Serialize(message));
    }
}