using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Todo.Projects.Commands;

namespace Todo.Projects
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services)
        {
            services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            services.AddSingleton<IProjectRepository, ProjectDynamoRepository>();

            return services;
        }
    }
}
