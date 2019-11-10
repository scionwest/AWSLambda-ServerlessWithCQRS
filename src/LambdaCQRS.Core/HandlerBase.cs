using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LambdaCQRS
{
    public class HandlerBase
    {
        public IConfiguration Configuration { get; protected set; }

        public IServiceProvider Services { get; protected set; }

        public async Task InitializeConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            await this.ConfigureHandler(configurationBuilder);
            this.Configuration = configurationBuilder.Build();
        }

        public async Task InitializeServiceProvider()
        {
            var services = new ServiceCollection();

            await this.RegisterHandlerServices(services);
            this.Services = services.BuildServiceProvider();
        }

        protected virtual Task ConfigureHandler(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: true);
            configurationBuilder.AddEnvironmentVariables();
            return Task.CompletedTask;
        }

        protected virtual Task RegisterHandlerServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(this.Configuration);
            return Task.CompletedTask;
        }
    }
}
