using Lambda.EventSource;
using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Todo.Projects.Commands;
using Todo.Projects.Domain;
using Todo.Projects.Dtos;

namespace Todo.Projects.Queries
{
    public class GetProjectQueryHandler : ApiGatewayQueryHandler
    {
        protected override Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddTodoServices();
            return base.RegisterHandlerServices(services);
        }

        protected override async Task<HandlerResponse> QueryHandler()
        {
            IProjectRepository repository = base.Services.GetRequiredService<IProjectRepository>();

            if (!this.ProxyRequest.Headers.TryGetValue("username", out string username))
            {
                username = "janedoe";
            }
            string projectId = base.ProxyRequest.PathParameters["projectId"];

            Project project = await repository.GetProjectByIdForUserAsync(username, projectId);
            GetProjectDto result = new GetProjectDto(project);

            //var bus = new EventBus();
            //await bus.PublishMessage(result, "TestPartition", "stream-name-from-config-here");
            return this.StatusOk(result);
        }
    }
}
