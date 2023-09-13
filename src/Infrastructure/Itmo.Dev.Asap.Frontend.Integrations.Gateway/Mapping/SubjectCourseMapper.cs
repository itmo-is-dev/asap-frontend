using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;
using Riok.Mapperly.Abstractions;
using CreateSubjectCourseRequest =
    Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses.CreateSubjectCourseRequest;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class SubjectCourseMapper
{
    public static partial CreateSubjectCourseRequest MapToRequest(
        this Application.Abstractions.SubjectCourses.Models.CreateSubjectCourseRequest request);

    [MapDerivedType<GithubCreateSubjectCourseArgs, CreateSubjectCourseGithubArgs>]
    public static partial CreateSubjectCourseArgs MapToArgs(GithubCreateSubjectCourseArgs args);
}