using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

namespace Itmo.Dev.Asap.Frontend.Presentation.Markup.StudentGroups.Models;

public class StudentGroupSearchItem
{
    public StudentGroupSearchItem(IStudentGroup group)
    {
        Group = group;
        Visible = true;
    }

    public IStudentGroup Group { get; }

    public bool Visible { get; set; }
}