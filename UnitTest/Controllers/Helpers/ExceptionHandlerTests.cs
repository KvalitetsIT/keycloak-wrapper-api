using System.Net;
using FS.Keycloak.RestApiClient.Client;
using Microsoft.AspNetCore.Mvc;

public class ExceptionHandlerTests
{
    [Test]
    public void HandleException_HttpRequestExceptionIsThrown_ReturnErrorWith202()
    {
        var exceptionHandler = new HttpExceptionHandler();
        var message = "specialKindOfErrorMessage";
        var innerException = new Exception();
        var httpCode = System.Net.HttpStatusCode.Accepted;

        var result = exceptionHandler.HandleException(new HttpRequestException(message, innerException, httpCode)) as JsonResult;
        var errorObj = result.Value as Error;

        Assert.AreEqual(message, errorObj.error);
        Assert.AreEqual("", errorObj.path);

        Assert.AreEqual((int)httpCode, result.StatusCode);
        Assert.AreEqual((int)httpCode, errorObj.status);
    }

    [Test]
    public void HandleException_ApiExceptionIsThrown_ReturnErrorWith202()
    {
        var exceptionHandler = new HttpExceptionHandler();
        var message = "specialKindOfErrorMessage";
        var innerException = new Exception();
        var httpCode = System.Net.HttpStatusCode.Accepted;

        var result = exceptionHandler.HandleException(new ApiException((int)httpCode, message)) as JsonResult;
        var errorObj = result.Value as Error;

        Assert.AreEqual(message, errorObj.error);
        Assert.AreEqual("", errorObj.path);

        Assert.AreEqual((int)httpCode, result.StatusCode);
        Assert.AreEqual((int)httpCode, errorObj.status);
    }

    [Test]
    public void HandleException_NullReferenceExceptionIsThrown_ReturnErrorWithInternalServerError()
    {
        var exceptionHandler = new HttpExceptionHandler();

        var result = exceptionHandler.HandleException(new NullReferenceException()) as JsonResult;
        var errorObj = result.Value as Error;

        Assert.AreEqual("Unknown error occured", errorObj.error);
        Assert.AreEqual("", errorObj.path);

        Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
    }
}