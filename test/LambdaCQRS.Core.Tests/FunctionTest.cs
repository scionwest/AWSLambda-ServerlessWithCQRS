using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Todo.Projects.Queries;
using Todo.Projects.Commands;
using Todo.Projects.Domain;

namespace Todo.Projects
{
    public class FunctionTest
    {

        [Fact]
        public async Task TestGetMethod()
        {
            // Arrange
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            request = new APIGatewayProxyRequest { Headers = new Dictionary<string, string>() };
            context = new TestLambdaContext();
            var functions = new GetProjectsQueryHandler();

            // Act
            response = await functions.RunHandler(request, context);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotEmpty(response.Body);
        }

        [Fact]
        public async Task TestGetByIdMethod()
        {
            // Arrange
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            request = new APIGatewayProxyRequest
            {
                Headers = new Dictionary<string, string>(),
                Path = $"/projects/0829c6f6-0cce-4c84-9d3b-dad62b533148",
                PathParameters = new Dictionary<string, string> { { "projectId", "0829c6f6-0cce-4c84-9d3b-dad62b533148"} }
            };
            context = new TestLambdaContext();
            var functions = new GetProjectQueryHandler();

            // Act
            response = await functions.RunHandler(request, context);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotEmpty(response.Body);
        }

        [Fact]
        public async Task TestCreateMethod()
        {
            // Arrange
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;
            string json = System.Text.Json.JsonSerializer.Serialize(new
            {
                Title = "Hello World",
                Priority = Prioritization.High.ToString(),
                PercentageCompleted = 50
            });

            request = new APIGatewayProxyRequest
            {
                Headers = new Dictionary<string, string>(),
                Body = json,
                Path = "/projects"
            };
            context = new TestLambdaContext();
            var functions = new CreateProjectCommandHandler();

            // Act
            response = await functions.RunHandler(request, context);

            // Assert
            Assert.Equal(201, response.StatusCode);
            Assert.Null(response.Body);
            Assert.NotEmpty(response.Headers["Location"]);
        }

        [Fact]
        public async Task TestDeleteMethod()
        {
            // Arrange
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;
            string json = System.Text.Json.JsonSerializer.Serialize(new
            {
                Title = "Hello World",
                Priority = Prioritization.High.ToString(),
                PercentageCompleted = 50
            });

            request = new APIGatewayProxyRequest
            {
                Headers = new Dictionary<string, string>(),
                Body = json,
                Path = "/projects",
                PathParameters = new Dictionary<string, string>()
            };

            context = new TestLambdaContext();
            var createCommand = new CreateProjectCommandHandler();
            var deleteCommand = new DeleteProjectCommandHandler();

            // Act
            response = await createCommand.RunHandler(request, context);
            string projectId = response.Headers["Location"];
            request.Body = null;
            request.PathParameters["projectId"] = projectId.Split("/").Last();
            request.Path = projectId;
            response = await deleteCommand.RunHandler(request, context);

            // Assert
            Assert.Equal(204, response.StatusCode);
            Assert.Null(response.Body);
            Assert.NotEmpty(response.Headers["Location"]);
        }
    }
}