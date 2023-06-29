using Microsoft.AspNetCore.Mvc;

public interface IExceptionHandler
{
    IActionResult HandleException(Exception exceptionToHandle);
}