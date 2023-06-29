using System.Net;

public class Error
{
    public string error { get; set; }
    public string path { get; set; } = "";
    public int status { get; set; }
    public DateTime timestamp { get; set; } = DateTime.Now;

    public Error(string error, HttpStatusCode? status)
    {
        var statusCode = status == null ? HttpStatusCode.InternalServerError : status;
        this.status = (int)statusCode;
        this.error = error;
    }
}