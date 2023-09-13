using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

[Mapper]
internal static partial class DtoMapper
{
    [MapDerivedType<GoogleSubjectCourseAssociationDto, GoogleSubjectCourseAssociation>]
    [MapDerivedType<GithubSubjectCourseAssociationDto, GithubSubjectCourseAssociation>]
    public static partial SubjectCourseAssociation MapToAssociation(SubjectCourseAssociationDto association);
}