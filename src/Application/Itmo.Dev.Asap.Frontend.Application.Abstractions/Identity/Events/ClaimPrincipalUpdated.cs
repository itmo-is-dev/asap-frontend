using Itmo.Dev.Asap.Frontend.Application.Events;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;

public record ClaimPrincipalUpdated(ClaimsPrincipal Principal) : IApplicationEvent;