namespace Api.Responces.Global;

public abstract class BaseResponce
{
    public int Code { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}