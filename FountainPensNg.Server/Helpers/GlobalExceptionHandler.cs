﻿namespace FountainPensNg.Server.Helpers;
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken) {
		logger.LogError(
	exception, "Exception occurred: {Message} at {Time}", exception.Message, DateTime.UtcNow);

		(string Detail, string Title, int StatusCode) details = exception switch {
			//ValidationException => (
			//	exception.Message,
			//	exception.GetType().Name,
			//	context.Response.StatusCode = StatusCodes.Status400BadRequest
			//),
			MappingException => (
				exception.Message,
				exception.GetType().Name,
				httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
			),
			NotFoundException => (
				exception.Message,
				exception.GetType().Name,
				httpContext.Response.StatusCode = StatusCodes.Status404NotFound
			),
			_ => (
				exception.Message,
				exception.GetType().Name,
				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
			)
		};
		var problemDetails = new ProblemDetails {
			Title = details.Title,
			Detail = details.Detail,
			Status = details.StatusCode,
			Instance = httpContext.Request.Path
		};
		problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
		//if (exception is ValidationException validationException) { //TODO when fluentValidation is implemented
		//	problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
		//}
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
		return true;
	}
}