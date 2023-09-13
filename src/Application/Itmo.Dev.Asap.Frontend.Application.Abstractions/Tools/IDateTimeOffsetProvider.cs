namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset Current { get; }
}