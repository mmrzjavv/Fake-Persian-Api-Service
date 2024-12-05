using System.Net;

namespace FakeApiFarsi.infrastructure.OperationRseult;

public class OperationResult<T>
{
    public bool Success { get; private set; } = false;
    public string OperationName { get; }
    public string Message { get; private set; }
    public string ExMessage { get; private set; }
    public DateTime OperationDate { get; } = DateTime.UtcNow;
    public HttpStatusCode Status { get; private set; } = HttpStatusCode.BadRequest;
    public T Object { get; private set; }
    public List<T> List { get; private set; }

    public OperationResult(string operationName)
    {
        OperationName = operationName ?? throw new ArgumentNullException(nameof(operationName));
    }

    // متد موفقیت
    public OperationResult<T> Succeed(string message, T obj = default, List<T> list = null,
        HttpStatusCode status = HttpStatusCode.OK)
    {
        Success = true;
        Message = message ?? string.Empty;
        Status = status;
        Object = obj;
        List = list;
        return this;
    }

    // متد شکست
    public OperationResult<T> Fail(string message, string exMessage = null, T obj = default, List<T> list = null,
        HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        Success = false;
        Message = message ?? string.Empty;
        ExMessage = exMessage;
        Status = status;
        Object = obj;
        List = list;
        return this;
    }
}