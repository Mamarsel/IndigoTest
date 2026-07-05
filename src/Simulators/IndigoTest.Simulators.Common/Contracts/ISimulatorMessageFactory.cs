using IndigoTest.Simulators.Common.MarketData;

namespace IndigoTest.Simulators.Common.Contracts;

public interface ISimulatorMessageFactory
{
    string Create(SimulatedTick tick);
}
