using Api.Responces.Globals;
using Domain.Generals;

namespace Api.Contracts;

public interface IResponceFactory
{
    ErrorResponce CreateErrorResponce(Result? result = null, Exception? ex = null);
    SuccessResponce<T> CreateSuccessResponce<T>(Result result, params T[] data);
}