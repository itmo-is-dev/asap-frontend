namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EagerDependencyPriorityAttribute : Attribute
{
    public EagerDependencyPriorityAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; }
}