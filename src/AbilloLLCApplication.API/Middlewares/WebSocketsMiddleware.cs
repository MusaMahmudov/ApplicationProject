using Microsoft.AspNetCore.Http;

namespace AbilloLLCApplication.API.Middlewares
{
    public class WebSocketsMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

      

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;

         
            if (request.Path.StartsWithSegments("/Hub", StringComparison.OrdinalIgnoreCase) &&
                request.Query.TryGetValue("accessToken", out var accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            await _next(context); ;
        }
    }
}
