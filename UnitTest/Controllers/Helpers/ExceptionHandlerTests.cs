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
}