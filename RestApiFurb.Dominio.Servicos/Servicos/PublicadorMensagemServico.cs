using RabbitMQ.Client;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Utils;
using System.Reflection;
using System.Text;
using System.Text.Json;

internal sealed class PublicadorMensagemServico : IPublicadorMensagemServico
{
    private readonly ConnectionFactory factory;

    public PublicadorMensagemServico(string hostName = "localhost")
    {
        factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = "guest",
            Password = "guest"
        };
    }

    public void Publicar<TMensagem>(TMensagem mensagem)
    {
        var fila = typeof(TMensagem).GetCustomAttribute<Fila>();
        if (fila == null)
            throw new InvalidOperationException($"Classe {typeof(TMensagem).Name} não possui o atributo [Fila].");

        var queueName = fila.Nome;

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);
    }
}
