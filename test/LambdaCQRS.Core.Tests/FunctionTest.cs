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

            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            var functions = new GetProjectsQuery();

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

            request = new APIGatewayProxyRequest();
            context = new TestLambdaContext();
            var functions = new GetProjectQuery();

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
                Type = ProjectType.List.ToString(),
                Status = Status.Active.ToString(),
                Priority = Prioritization.High.ToString(),
                PercentageCompleted = 50
            });

            request = new APIGatewayProxyRequest
            {
                Body = json,
                Path = "/projects"
            };
            context = new TestLambdaContext();
            var functions = new CreateProjectCommand();

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
                Type = ProjectType.List.ToString(),
                Status = Status.Active.ToString(),
                Priority = Prioritization.High.ToString(),
                PercentageCompleted = 50
            });

            request = new APIGatewayProxyRequest
            {
                Body = json,
                Path = "/projects",
                PathParameters = new Dictionary<string, string>()
            };
            context = new TestLambdaContext();
            var createCommand = new CreateProjectCommand();
            var deleteCommand = new DeleteProjectCommand();

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