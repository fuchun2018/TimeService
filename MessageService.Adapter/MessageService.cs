using MessageService.In;
using MessageServiceProvider.@out;

namespace MessageService.Adapter;

public class MessageService : IMessageService
{
    private readonly IMessageServiceProvider _messageServiceProvider;

    public MessageService(IMessageServiceProvider messageServiceProvider)
    {
        _messageServiceProvider = messageServiceProvider;
    }

    public async Task SendAsync(string message)
    {
        await _messageServiceProvider.SendAsync(message);
    }
}