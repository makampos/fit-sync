namespace FitSync.Domain.Responses;

public class ServiceResponse<T>
{
    private ServiceResponse(bool success, T? data, string? errorMessage)
    {
        Success = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static ServiceResponse<T> SuccessResult(T data)
    {
        return new ServiceResponse<T>(true, data, null);
    }

    public static ServiceResponse<T> FailureResult(string errorMessage)
    {
        return new ServiceResponse<T>(false, default, errorMessage);
    }
}