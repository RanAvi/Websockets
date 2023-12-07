using System.Runtime.CompilerServices;
using System.Text;

namespace Websockets
{
    public static class Ioc
    {

        public static void ConfigureIdentityModule(this WebApplicationBuilder builder)
        {
            AddServices(builder.Services);
            ConfigurationProviders(builder);

        }

        public static async Task ApplicationIntilaize(this WebApplication app)
        {
            var websocketList = new WebsocketList();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(30) });

          app.UseEndpoints(endpoints =>
            {
           endpoints.Map("/ws", async ctx =>
                {
                    var buffer = new byte[1024 * 4];
                    var webSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                    websocketList._connections.Add(webSocket);

                    var result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);
                    int i = 0;
                    while (!result.CloseStatus.HasValue)
                    {
                        var message = Encoding.UTF8.GetBytes($"message index {i++}");
                        foreach (var c in websocketList._connections)
                        {
                            await c.SendAsync(new(message, 0, message.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        }

                        result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);

                        Console.WriteLine($"Received: {Encoding.UTF8.GetString(buffer[..result.Count])}");
                    }

                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    websocketList._connections.Remove(webSocket);
                });
            });

        }

        private static void ConfigurationProviders(WebApplicationBuilder builder)
        {
            

        }

        private static void AddServices(IServiceCollection services)
        {

    
        }




    }
}
