using System.Threading.Tasks;
using Todo.Projects.Domain;

namespace Todo.Projects.Commands
{
    public interface IProjectRepository
    {
        Task CreateProjectAsync(string username, Project newProject);
        Task DeleteProjectAsync(string username, string projectId);

        Task<Project[]> GetProjectsForUserAsync(string username);
        Task<Project> GetProjectByIdForUserAsync(string username, string projectId);
    }
}
