using Amazon.Lambda.Core;
using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Todo.Projects.Commands;
using Todo.Projects.Domain;
using Todo.Projects.Dtos;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Todo.Projects.Queries
{
    public class GetProjectsQueryHandler : ApiGatewayQueryHandler
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

            Project[] projects = await repository.GetProjectsForUserAsync(username);
            GetProjectsDto result = new GetProjectsDto(projects);
            return this.StatusOk(result);
        }
    }
}
