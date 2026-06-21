using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestApiFurb.Dominio.Utils;
using System.Reflection;
using System.Text;
using System.Text.Json;

internal abstract class ConsumidorBase<TMensagem> : BackgroundService
{
    private readonly ConnectionFactory factory;

    protected ConsumidorBase(string hostName = "localhost")
    {
        factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = "guest",
            Password = "guest"
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var fila = typeof(TMensagem).GetCustomAttribute<Fila>()
                  ?? throw new InvalidOperationException($"Classe {typeof(TMensagem).Name} não possui [Fila].");

        var queueName = fila.Nome;
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var mensagem = JsonSerializer.Deserialize<TMensagem>(json);

                if (mensagem != null)
                {
                    await ConsumirMensagem(mensagem);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: false,
                             consumer: consumer);

        stoppingToken.Register(() =>
        {
            channel.Close();
            connection.Close();
        });

        return Task.CompletedTask;
    }

    public abstract Task ConsumirMensagem(TMensagem mensagem);
}
