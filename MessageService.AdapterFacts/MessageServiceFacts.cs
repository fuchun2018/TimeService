using MessageServiceProvider.@out;
using Moq;

namespace MessageService.Adapter.Facts;

public class MessageServiceFacts
{
    [Fact]
    public async Task SendAsync_Successful()
    {
        // Arrange
        var message = "Test Message";
        var serviceProviderMock = new Mock<IMessageServiceProvider>();
        serviceProviderMock
            .Setup(provider => provider.SendAsync(message)).Verifiable();

        var messageService = new MessageService(serviceProviderMock.Object);

        // Act
        await messageService.SendAsync(message);

        // Assert
        serviceProviderMock.Verify(
            provider => provider.SendAsync(message),
            Times.Once); // Verify that SendAsync was called once
    }

    [Fact]
    public async Task SendAsync_ExceptionHandling()
    {
        // Arrange
        var message = "Test Message";
        var serviceProviderMock = new Mock<IMessageServiceProvider>();
        serviceProviderMock
            .Setup(provider => provider.SendAsync(message))
            .ThrowsAsync(new Exception("Sending message failed"));

        var messageService = new MessageService(serviceProviderMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => messageService.SendAsync(message));
        serviceProviderMock.Verify(
            provider => provider.SendAsync(message),
            Times.Once); // Verify that SendAsync was called once
    }
}