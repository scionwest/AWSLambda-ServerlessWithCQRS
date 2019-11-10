using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;

namespace LambdaCQRS
{
    public class ApiGatewayQueryHandler : ApiGatewayHandler
    {
        public async Task<APIGatewayProxyResponse> RunHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            await this.Initialize(request, context);

            // Run query
            HandlerResponse response = await this.QueryHandler();

            // Response to client
            APIGatewayProxyResponse httpResponse = await base.CreateGatewayResponse(response);
            return httpResponse;
        }

        protected virtual Task<HandlerResponse> QueryHandler()
            => throw new NotImplementedException("This method is not to be invoked on the base class. Override it with an implementation.");

        protected override string GetDefaultContentType() => "application/json";
    }
}
