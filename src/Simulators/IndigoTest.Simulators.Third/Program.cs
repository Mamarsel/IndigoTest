using IndigoTest.Simulators.Common.Composition;
using IndigoTest.Simulators.Third.ThirdExchange;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSimulator<ThirdExchangeMessageFactory>();

var app = builder.Build();

app.UseWebSockets();
app.MapSimulatorEndpoints();
app.Run();
