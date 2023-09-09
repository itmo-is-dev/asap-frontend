using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Extensions;

public static class SubjectCourseAssociationExtensions
{
    public static int GetOrdinal(this SubjectCourseAssociation association)
    {
        return association switch
        {
            GithubSubjectCourseAssociation githubSubjectCourseAssociation => 1,
            GoogleSubjectCourseAssociation googleSubjectCourseAssociation => 2,
            _ => int.MaxValue,
        };
    }
}