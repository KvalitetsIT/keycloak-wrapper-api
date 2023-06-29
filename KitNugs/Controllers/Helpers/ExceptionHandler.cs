using Microsoft.AspNetCore.Mvc;

public class HttpExceptionHandler : IExceptionHandler
{
    public IActionResult HandleException(Exception exceptionToHandle)
    {
        try
        {
            throw exceptionToHandle;
        }
        catch (HttpRequestException e)
        {
            var errorModel = new Error(e.Message, e.StatusCode);
            return new JsonResult(errorModel)
            {
                StatusCode = (int)e.StatusCode
            };
        };
    }
}