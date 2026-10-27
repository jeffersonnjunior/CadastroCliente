namespace Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "credito.clientes";
    public string RoutingKey { get; set; } = "cliente.cadastrado";
    public bool Enabled { get; set; } = true;
}
