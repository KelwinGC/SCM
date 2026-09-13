using System.ComponentModel.DataAnnotations;

namespace SCM.ApiControl.Infraestructura.EventBus.Options;

public sealed class RabbitMqSettings
{
    //public const string SectionName = "RabbitMq";
    public const string SectionName = "RabbitMqOptions";

    [Required]
    public string HostName { get; set; } = "localhost";

    [Range(1, 65535)]
    public ushort Port { get; set; } = 5672;

    [Required]
    public string UserName { get; set; } = "guest";

    [Required]
    public string Password { get; set; } = "guest";

    [Required]
    public string VirtualHost { get; set; } = "/";

    [Required]
    public string QueueName { get; set; } = "demo.message-created";

    [Range(1, 1000)]
    public ushort PrefetchCount { get; set; } = 16;

    [Range(0, 20)]
    public int RetryCount { get; set; } = 3;

    [Range(1, 3600)]
    public int RetryIntervalSeconds { get; set; } = 5;
}
