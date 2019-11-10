using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace LambdaCQRS
{
    public class ApiGatewayHandler : HandlerBase
    {
        public APIGatewayProxyRequest ProxyRequest { get; set; }

        protected ILambdaLogger Logger { get; set; }

        public async Task Initialize(APIGatewayProxyRequest request, ILambdaContext context)
        {
            this.ProxyRequest = request;
            this.Logger = context.Logger;
            await this.InitializeConfiguration();
            await this.InitializeServiceProvider();
        }

        protected virtual string GetDefaultContentType() => "application/json";

        protected virtual Task<string> SerializeResponseBody(object data)
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(data));
        }

        protected virtual Task<TData> DeserializeResponseBody<TData>(string json)
        {
            TData data = System.Text.Json.JsonSerializer.Deserialize<TData>(json);
            return Task.FromResult(data);
        }

        protected virtual async Task<APIGatewayProxyResponse> CreateGatewayResponse(HandlerResponse response)
        {
            if (!response.Headers.ContainsKey("Content-Type"))
            {
                response.Headers["Content-Type"] = this.GetDefaultContentType();
            }

            var httpResponse = new APIGatewayProxyResponse
            {
                Headers = response.Headers,
                StatusCode = response.HttpStatusCode
            };

            if (response.Data != null)
            {
                httpResponse.Body = await this.SerializeResponseBody(response.Data);
            }

            return httpResponse;
        }
    }
}
