﻿using LambdaCQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Todo.Projects.Commands
{
    public class DeleteProjectCommand : ApiGatewayCommandHandler
    {
        protected override Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddTodoServices();
            return base.RegisterHandlerServices(services);
        }

        protected override async Task<HandlerResponse> CommandHandler()
        {
            IProjectRepository repository = base.Services.GetRequiredService<IProjectRepository>();
            if (!this.ProxyRequest.Headers.TryGetValue("username", out string username))
            {
                username = "janedoe";
            }

            string projectId = base.ProxyRequest.PathParameters["projectId"];
            await repository.DeleteProjectAsync(username, projectId);

            return this.StatusDeleted(projectId);
        }
    }
}
