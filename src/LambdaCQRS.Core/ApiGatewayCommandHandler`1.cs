using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Threading.Tasks;

namespace LambdaCQRS
{
    public class ApiGatewayCommandHandler<TRequestBodyData> : ApiGatewayHandler where TRequestBodyData : class, new()
    {
        public async Task<APIGatewayProxyResponse> RunHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            await this.Initialize(request, context);

            // Run query
            TRequestBodyData requestBody = await base.DeserializeResponseBody<TRequestBodyData>(base.ProxyRequest.Body);
            HandlerResponse response = await this.CommandHandler(requestBody);

            // Response to client
            APIGatewayProxyResponse httpResponse = await this.CreateGatewayResponse(response);
            return httpResponse;
        }

        protected virtual Task<HandlerResponse> CommandHandler(TRequestBodyData requestBody)
            => throw new NotImplementedException("This method is not to be invoked on the base class. Override it with an implementation.");

        protected override string GetDefaultContentType() => "application/plaintext";
    }
}
