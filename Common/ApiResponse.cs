using Microsoft.AspNetCore.Mvc;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Meta { get; set; }
   // public List<string>? Errors { get; set; }

    // 200 OK - Success with data
    public static ActionResult<ApiResponse<T>> Ok(T data, string? message = null)
    {
        var res = new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
        return new OkObjectResult(res);
    }

    // 201 Created - Resource created successfully
    public static ActionResult<ApiResponse<T>> Created(T data, string location)
    {
        var res = new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data
        };
        return new CreatedResult(location, res); // URI can be provided if needed
    }

    // 204 No Content - Success but no data to return
    public static ActionResult<ApiResponse<string>> NoContent(string? message = null)
    {
        var res = new ApiResponse<string>
        {
            IsSuccess = true,
            Message = message ?? "No content."
        };
        return new OkObjectResult(res); 
    }
    
    // Paginated response (200 OK)
    public static ActionResult<ApiResponse<T>> Paginated(T data, object meta, string? message = null)
    {
        var res = new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            Meta = meta
        };
        return new OkObjectResult(res);
    }
    
    
    // Error responses --> Done by Exception handler
    // 500 Internal Server Error - Unexpected server error
    /*public static ActionResult<ApiResponse<T>> InternalServerError(string message = "An unexpected error occurred.")
    {
        var res = new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message
        };
        return new ObjectResult(res) { StatusCode = 500 };
    }*/

    
}