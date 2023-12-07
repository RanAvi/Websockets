using System.Net.WebSockets;
using Websockets;



var builder = WebApplication.CreateBuilder(args);
builder.ConfigureIdentityModule();









var app = builder.Build();
 await app.ApplicationIntilaize();



app.Run();
