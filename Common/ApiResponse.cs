namespace Smart_Home_IoT_Device_Management_API.Common;

public class ApiResponse<T>
{
    private bool IsSuccess { get; set; }
    private string? Message { get; set; }
    private T? Data { get; set; }
    private object? Meta { get; set; }
    private List<string>? Errors { get; set; }
    
    // Success without meta
    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
        
    //  Failure with message and optional errors
    public static ApiResponse<T> Fail(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
    
    public static ApiResponse<T> Paginated(T data, object meta, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Meta = meta
            };
        }
}

    
