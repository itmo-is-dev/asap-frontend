using Itmo.Dev.Asap.Frontend.Presentation.Components.Values;

namespace Itmo.Dev.Asap.Frontend.Presentation.Components.Parameters;

public record struct ParameterContext<T>(
    IParameterController<T> ParameterController,
    IValuesController ValuesController);