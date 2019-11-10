using System;
using System.Net;

namespace LambdaCQRS
{
    public static class ApiGatewayHandlerExtensions
    {
        public static HandlerResponse StatusOk<TData>(this ApiGatewayHandler handler, TData data)
        {
            var response = new HandlerResponse(data, (int)HttpStatusCode.OK);
            return response;
        }
        public static HandlerResponse StatusCreated(this ApiGatewayHandler handler, int resourceId)
            => handler.StatusCreated(resourceId.ToString());

        public static HandlerResponse StatusCreated(this ApiGatewayHandler handler, Guid resourceId)
            => handler.StatusCreated(resourceId.ToString());

        public static HandlerResponse StatusCreated(this ApiGatewayHandler handler, string resourceId)
        {
            var response = new HandlerResponse((int)HttpStatusCode.Created);
            response.Headers["Location"] = $"{handler.ProxyRequest.Path}{(handler.ProxyRequest.Path.EndsWith("/") ? "" : "/")}{resourceId}";
            return response;
        }

        public static HandlerResponse StatusUpdated(this ApiGatewayHandler handler, int resourceId)
            => handler.StatusUpdated(resourceId.ToString());

        public static HandlerResponse StatusUpdated(this ApiGatewayHandler handler, Guid resourceId)
            => handler.StatusUpdated(resourceId.ToString());

        public static HandlerResponse StatusUpdated(this ApiGatewayHandler handler, string resourceId)
        {
            var response = new HandlerResponse((int)HttpStatusCode.NoContent);
            response.Headers["Location"] = $"{handler.ProxyRequest.Path}{(handler.ProxyRequest.Path.EndsWith("/") ? "" : "/")}{resourceId}";
            return response;
        }

        public static HandlerResponse StatusPatched(this ApiGatewayHandler handler, int resourceId)
            => handler.StatusPatched(resourceId.ToString());

        public static HandlerResponse StatusPatched(this ApiGatewayHandler handler, Guid resourceId)
            => handler.StatusPatched(resourceId.ToString());

        public static HandlerResponse StatusPatched(this ApiGatewayHandler handler, string resourceId)
        {
            var response = new HandlerResponse((int)HttpStatusCode.NoContent);
            response.Headers["Location"] = $"{handler.ProxyRequest.Path}{(handler.ProxyRequest.Path.EndsWith("/") ? "" : "/")}{resourceId}";
            return response;
        }

        public static HandlerResponse StatusDeleted(this ApiGatewayHandler handler, int resourceId)
            => handler.StatusDeleted(resourceId.ToString());

        public static HandlerResponse StatusDeleted(this ApiGatewayHandler handler, Guid resourceId)
            => handler.StatusDeleted(resourceId.ToString());

        public static HandlerResponse StatusDeleted(this ApiGatewayHandler handler, string resourceId)
        {
            var response = new HandlerResponse((int)HttpStatusCode.NoContent);
            response.Headers["Location"] = $"{handler.ProxyRequest.Path}{(handler.ProxyRequest.Path.EndsWith("/") ? "" : "/")}{resourceId}";
            return response;
        }
    }
}
