namespace VacationRental.Middleware.ExceptionHandling;

/// <summary>
/// Is used to specify an endoint on which <see cref="ExceptionHandlingMiddleware">ExceptionHandlingMiddleware</see> should be used.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HandleExceptionsAttribute : Attribute
{
}
