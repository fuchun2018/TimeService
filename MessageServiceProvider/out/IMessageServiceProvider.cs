namespace MessageServiceProvider.@out;

public interface IMessageServiceProvider
{
    Task SendAsync(string message);
}