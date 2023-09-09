using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;
using Itmo.Dev.Asap.Frontend.Application.Assignments;
using Itmo.Dev.Asap.Frontend.Application.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Identity;
using Itmo.Dev.Asap.Frontend.Application.Queues;
using Itmo.Dev.Asap.Frontend.Application.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Students;
using Itmo.Dev.Asap.Frontend.Application.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Tools;
using Itmo.Dev.Asap.Frontend.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        collection.AddSingleton<ICurrentUser, CurrentUser>();

        collection.AddSingleton<ISubjectList, SubjectList>();
        collection.AddSingleton<ISubjectFactory, SubjectFactory>();
        collection.AddSingleton<ISubjectProvider, SubjectProvider>();

        collection.AddSingleton<ISubjectCourseList, SubjectCourseList>();
        collection.AddSingleton<ISubjectCourseFactory, SubjectCourseFactory>();
        collection.AddSingleton<ISubjectCourseProvider, SubjectCourseProvider>();
        collection.AddSingleton<ISubjectCourseListProvider, SubjectCourseListProvider>();

        collection.AddSingleton<IAssignmentFactory, AssignmentFactory>();
        collection.AddSingleton<IAssignmentListProvider, AssignmentListProvider>();

        collection.AddSingleton<IGroupAssignmentFactory, GroupAssignmentFactory>();
        collection.AddSingleton<IGroupAssignmentListProvider, GroupAssignmentListProvider>();

        collection.AddSingleton<ISubjectCourseGroupFactory, SubjectCourseGroupFactory>();
        collection.AddSingleton<ISubjectCourseGroupListProvider, SubjectCourseGroupListProvider>();

        collection.AddSingleton<IStudentFactory, StudentFactory>();

        collection.AddSingleton<IStudentGroupFactory, StudentGroupFactory>();

        collection.AddSingleton<IIdentityUserFactory, IdentityUserFactory>();

        collection.AddSingleton<IQueueSubmissionFactory, QueueSubmissionFactory>();
        collection.AddSingleton<IQueueProvider, QueueProvider>();
        collection.AddSingleton<IQueueListProvider, QueueListProvider>();

        collection.AddSingleton<PrincipalParserTokenHandler>();
        collection.AddSingleton<PrincipalExpirationHandler>();

        return collection;
    }
}