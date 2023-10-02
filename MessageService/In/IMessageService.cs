namespace MessageService.In;

public interface IMessageService
{
    Task SendAsync(string message);
}