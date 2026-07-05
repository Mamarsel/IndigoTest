using IndigoTest.Simulators.Common.Composition;
using IndigoTest.Simulators.First.FirstExchange;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSimulator<FirstExchangeMessageFactory>();

var app = builder.Build();

app.UseWebSockets();
app.MapSimulatorEndpoints();
app.Run();
