namespace Company.Template.Infrastructure.Messaging;

/// <summary>
/// Interface for publishing messages to message broker
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified exchange
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="message">Message to publish</param>
    /// <param name="exchange">Exchange name</param>
    /// <param name="routingKey">Routing key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default);
}