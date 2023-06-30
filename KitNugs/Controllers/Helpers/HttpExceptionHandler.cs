using System.Net;
using FS.Keycloak.RestApiClient.Client;
using Microsoft.AspNetCore.Mvc;

public class HttpExceptionHandler : IExceptionHandler
{
    public IActionResult HandleException(Exception exceptionToHandle)
    {
        try
        {
            throw exceptionToHandle;
        }
        catch (HttpRequestException httpException)
        {
            var errorModel = new Error(httpException.Message, (int?)httpException.StatusCode);
            return new JsonResult(errorModel)
            {
                StatusCode = (int?)httpException.StatusCode
            };
        }
        catch (ApiException apiException)
        {

            var errorModel = new Error(apiException.Message, apiException.ErrorCode);
            return new JsonResult(errorModel)
            {
                StatusCode = apiException.ErrorCode
            };
        }
        catch (Exception exception)
        {
            var errorcode = HttpStatusCode.InternalServerError;
            var errorMessage = "Unknown error occured";
            var errorModel = new Error(errorMessage, (int)errorcode);
            return new JsonResult(errorModel)
            {
                StatusCode = (int)errorcode
            };
        }
    }
}