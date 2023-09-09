using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;

public static class CurrentUserExtensions
{
    private const string Admin = "admin";
    private const string Moderator = "moderator";
    private const string Mentor = "mentor";

    public static IObservable<bool> IsInRole(this ICurrentUser user, string role)
    {
        return user.Roles.Select(x => x.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)));
    }

    public static IObservable<bool> IsAdmin(this ICurrentUser user)
    {
        return user.IsInRole(Admin);
    }

    public static IObservable<bool> IsModerator(this ICurrentUser user)
    {
        return user.IsInRole(Moderator);
    }

    public static IObservable<bool> IsMentor(this ICurrentUser user)
    {
        return user.IsInRole(Mentor);
    }

    public static IObservable<bool> HasModeratorAccess(this ICurrentUser user)
    {
        return user.Roles.Select(x => x.Any(r =>
            r.Equals(Moderator, StringComparison.OrdinalIgnoreCase)
            || r.Equals(Admin, StringComparison.OrdinalIgnoreCase)));
    }

    public static IObservable<bool> HasMentorAccess(this ICurrentUser user)
    {
        return user.Roles.Select(x => x.Any(r =>
            r.Equals(Moderator, StringComparison.OrdinalIgnoreCase)
            || r.Equals(Admin, StringComparison.OrdinalIgnoreCase)
            || r.Equals(Mentor, StringComparison.OrdinalIgnoreCase)));
    }
}