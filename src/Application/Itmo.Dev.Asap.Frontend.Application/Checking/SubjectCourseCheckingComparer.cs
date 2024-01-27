using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Checking;

public class SubjectCourseCheckingComparer : IEqualityComparer<SubjectCourseChecking>
{
    public static readonly SubjectCourseCheckingComparer Instance = new SubjectCourseCheckingComparer();

    public bool Equals(SubjectCourseChecking? x, SubjectCourseChecking? y)
    {
#pragma warning disable IDE0072
        return (x, y) switch
#pragma warning restore IDE0072
        {
            (null, null) => true,
            (not null, null) or (null, not null) => false,
            ({ Id: var left }, { Id: var right }) => left.Equals(right),
        };
    }

    public int GetHashCode(SubjectCourseChecking obj)
    {
        return obj.Id.GetHashCode();
    }
}