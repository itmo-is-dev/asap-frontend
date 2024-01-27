namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;

public record struct StudentData(Guid Id, string FirstName, string LastName, string GroupName)
{
    public string DisplayName => $"{LastName} {FirstName} ({GroupName})";
}