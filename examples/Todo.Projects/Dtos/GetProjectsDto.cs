using System.Linq;
using Todo.Projects.Domain;

namespace Todo.Projects.Dtos
{
    public class GetProjectsDto
    {
        public GetProjectsDto(Project[] projects)
        {
            this.Projects = projects
                .Select(project => new GetProjectDto(project))
                .ToArray();
        }

        public GetProjectDto[] Projects { get; }
    }
}
