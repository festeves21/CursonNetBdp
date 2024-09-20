﻿using Newtonsoft.Json;
using System.Net;
using WebAppDia3.Models;

namespace WebAppDia3.infreaestructura
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Unhandled exception occurred");

                // Set the response properties
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Create a response model
                var response = new ErrorResponse
                {
                    Message = "An unexpected error occurred. Please try again later.",
                    Detail = ex.Message // Consider using a more secure approach for production
                };

                // Serialize and write the response
                var jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
