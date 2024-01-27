namespace Itmo.Dev.Asap.Frontend.Presentation.Components.Parameters;

public interface IParameterController<T>
{
    IEnumerable<QueryValue<T>> Values { get; }

    Task OnValuesChangedAsync();

    void AddValue(QueryValue<T> value);

    string ConvertValueToString(T value);

    void RemoveValue(QueryValue<T> value);
}