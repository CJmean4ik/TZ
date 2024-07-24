using Api.Responces.Global;

namespace Api.Responces.Globals;

public class SuccessResponce<T> : BaseResponce
{
    public List<T>? Data { get; set; }
    
}