using System;
using System.Collections.Generic;

namespace LambdaCQRS
{
    public class HandlerResponse
    {
        public HandlerResponse(object data, int statusCode) : this(statusCode)
        {
            this.Data = data;
        }

        public HandlerResponse(int statusCode, params string[] errors)
        {
            this.HttpStatusCode = statusCode;
            this.Errors = errors ?? Array.Empty<string>();
        }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public int HttpStatusCode { get; }
        public string[] Errors { get; }
        public bool IsSuccessful => this.Errors.Length == 0;
        public virtual object Data { get; }
    }
}
