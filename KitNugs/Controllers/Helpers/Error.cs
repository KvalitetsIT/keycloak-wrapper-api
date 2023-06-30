using System.Net;

public class Error
{
    public string error { get; set; }
    public string path { get; set; } = "";
    public int status { get; set; }
    public DateTime timestamp { get; set; } = DateTime.Now;

    public Error(string error, int? status)
    {
        var statusCode = status == null ? (int)HttpStatusCode.InternalServerError : status;
        this.status = (int)statusCode;
        this.error = error;
    }
}