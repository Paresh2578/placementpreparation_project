using backend.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using backend.Middlewares;

namespace backend.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, List<DateTime>> _requestLogs = new();

        private readonly int _maxRequests = 1; // Max 10 Requests
        private readonly TimeSpan _timeWindow = TimeSpan.FromSeconds(5); // Time Window 5 Seconds

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(clientIp))
            {
                await _next(context);
                return;
            }

            if (!_requestLogs.ContainsKey(clientIp))
            {
                _requestLogs[clientIp] = new List<DateTime>();
            }

            _requestLogs[clientIp].Add(DateTime.UtcNow);

            // Remove Old Requests Outside Time Window
            _requestLogs[clientIp].RemoveAll(t => t < DateTime.UtcNow - _timeWindow);

            if (_requestLogs[clientIp].Count > _maxRequests)
            {
                context.Response.StatusCode = 429; // Too Many Requests
                // await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel{StatusCode=429 , Message="To Many Requests in 10 secound . Try again after 10 secound"}));
                await context.Response.WriteAsync("To Many Requests in 10 secound . Try again after 10 secound");
                return;
            }

            await _next(context);
        }
    }
}
// Extension method used to add the middleware to the HTTP request pipeline.
public static class RateLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimitMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitMiddleware>();
    }
}