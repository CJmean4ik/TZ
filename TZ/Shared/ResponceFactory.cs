using Api.Contracts;
using Api.Responces.Globals;
using Domain.Generals;

namespace Api.Shared;

public class ResponceFactory : IResponceFactory
{
    public ErrorResponce CreateErrorResponce(Result? result = null, Exception? ex = null) =>
       new ErrorResponce()
        {
            Code = StatusCodes.Status500InternalServerError,
            Title = "Error on server occured",
            Message = DefineErrorMessage(result,ex),
            Error = result is not null
                ? result.Error
                : new Error(ErrorCodes.GlobalError, "An internal error has occurred")
        };

    public SuccessResponce<T> CreateSuccessResponce<T>(Result result, params T[] data) => 
        new SuccessResponce<T>
        {
            Code = StatusCodes.Status200OK,
            Title = "The operation was successful",
            Message = result.Message,
            Data = new List<T>(data)
        };
    
    private static string DefineErrorMessage(Result? result, Exception? exception)
    {
        if (exception is null && result is not null)
            return result.Message;
        
        if (exception is not null && result is null)
            return exception.Message;
        
        return $"Error occured: Result.Message: {result.Message} Exception.Message: {result.Message}";
    }
}