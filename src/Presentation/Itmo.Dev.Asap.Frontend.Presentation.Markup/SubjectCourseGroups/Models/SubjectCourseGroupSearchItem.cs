using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

namespace Itmo.Dev.Asap.Frontend.Presentation.Markup.SubjectCourseGroups.Models;

public class SubjectCourseGroupSearchItem
{
    public SubjectCourseGroupSearchItem(IStudentGroup group)
    {
        Group = group;
        Visible = true;
    }

    public IStudentGroup Group { get; }

    public bool Visible { get; set; }
}