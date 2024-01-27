using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Github;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Tools;
using Itmo.Dev.Asap.Gateway.Sdk.Authentication;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayIntegration(this IServiceCollection collection)
    {
        collection.AddAsapSdk();
        collection.AddSingleton<ITokenProvider, TokenProvider>();

        collection.AddSingleton<ISubjectService, SubjectService>();
        collection.AddSingleton<ISubjectCourseService, SubjectCourseService>();
        collection.AddSingleton<IAssignmentService, AssignmentService>();
        collection.AddSingleton<IGroupAssignmentService, GroupAssignmentService>();
        collection.AddSingleton<ISubjectCourseGroupService, SubjectCourseGroupService>();
        collection.AddSingleton<IStudentGroupService, StudentGroupService>();
        collection.AddSingleton<IStudentService, StudentService>();
        collection.AddSingleton<IUserService, UserService>();
        collection.AddSingleton<IQueueService, QueueService>();

        collection.AddSingleton<IIdentityService, IdentityService>();
        collection.AddSingleton<IGithubService, GithubService>();

        collection.AddSingleton<ICheckingService, CheckingService>();
        collection.AddSingleton<ICheckingResultService, CheckingResultService>();
        collection.AddSingleton<ICheckingCodeBlocksService, CheckingCodeBlocksService>();

        collection.AddSingleton<ISubmissionService, SubmissionService>();

        return collection;
    }
}