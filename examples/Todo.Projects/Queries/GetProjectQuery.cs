using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Todo.Projects.Commands;
using Todo.Projects.Domain;

namespace Todo.Projects.Queries
{
    public class GetProjectQuery : ApiGatewayQueryHandler
    {
        protected override Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddTodoServices();
            return base.RegisterHandlerServices(services);
        }

        protected override async Task<HandlerResponse> QueryHandler()
        {
            IProjectRepository repository = base.Services.GetRequiredService<IProjectRepository>();

            string username = this.ProxyRequest.Headers["username"] ?? "janedoe";
            string projectId = base.ProxyRequest.PathParameters["projectId"];

            Project project = await repository.GetProjectByIdForUserAsync(username, projectId);

            return this.StatusOk(project);
        }
    }
}
