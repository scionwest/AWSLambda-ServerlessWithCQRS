using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Todo.Projects.Domain;
using Todo.Projects.Dtos;

namespace Todo.Projects.Commands
{
    public class CreateProjectCommandHandler : ApiGatewayCommandHandler<CreateProjectCommand>
    {
        protected override Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddTodoServices();
            return base.RegisterHandlerServices(services);
        }

        protected override async Task<HandlerResponse> CommandHandler(CreateProjectCommand requestBody)
        {
            IProjectRepository repository = base.Services.GetRequiredService<IProjectRepository>();
            if (!this.ProxyRequest.Headers.TryGetValue("username", out string username))
            {
                username = "janedoe";
            }

            var newProject = new Project(requestBody.Id, "janedoe", requestBody.Priority, requestBody.Title);
            if (requestBody.IsFlagged)
            {
                newProject.FlagProject();
            }
            newProject.SetPercentageComplete(requestBody.PercentageCompleted);

            if (requestBody.StartDate.HasValue)
            {
                newProject.StartProject(requestBody.StartDate.Value);
            }

            if (requestBody.TargetDate.HasValue)
            {
                newProject.SetTargetDate(requestBody.TargetDate.Value);
            }

            try
            {
                await repository.CreateProjectAsync(username, newProject);
                return this.StatusCreated(newProject.Id);
            }
            catch (System.Exception ex)
            {
                this.Logger.LogLine(ex.Message);
                return new HandlerResponse((int)HttpStatusCode.BadRequest);
            }
        }
    }
}
