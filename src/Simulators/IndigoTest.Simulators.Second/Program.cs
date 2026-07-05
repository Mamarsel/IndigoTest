using IndigoTest.Simulators.Common.Composition;
using IndigoTest.Simulators.Second.SecondExchange;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services.AddSimulator<SecondExchangeMessageFactory>();

var app = builder
    .Build();

app.UseWebSockets();

app.MapSimulatorEndpoints();

app.Run();
