namespace Api;

public class Response<T>
{
    public string? Message { get; set; }
    public int? Code { get; set; }
    public bool Succeed { get; set; }
    public T? Result { get; set; }

    public static Response<T> CreateSuccessful(T? body)
    {
        return new Response<T>
        {
            Code = 200,
            Succeed = true,
            Result = body,
            Message = "Succeed"
        };
    }
    
    public static Response<T> CreateSuccessful()
    {
        return new Response<T>
        {
            Code = 200,
            Succeed = true,
            Message = "Succeed"
        };
    }
    
    public static Response<T> CreateFailed(string errorMessage)
    {
        return new Response<T>
        {
            Code = 400,
            Succeed = false,
            Message = errorMessage
        };
    }
}