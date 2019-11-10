using Amazon.Lambda.Core;
using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Todo.Projects.Commands;
using Todo.Projects.Domain;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Todo.Projects.Queries
{
    public class GetProjectsQuery : ApiGatewayQueryHandler
    {
        protected override Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddTodoServices();
            return base.RegisterHandlerServices(services);
        }

        protected override async Task<HandlerResponse> QueryHandler()
        {
            IProjectRepository repository = base.Services.GetRequiredService<IProjectRepository>();
            string user = "janedoe";

            Project[] projects = await repository.GetProjectsForUserAsync(user);

            return this.StatusOk(projects);
        }
    }
}
