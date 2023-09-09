using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;

namespace Itmo.Dev.Asap.Frontend.Application.Tools;

internal class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Current => DateTimeOffset.UtcNow;
}