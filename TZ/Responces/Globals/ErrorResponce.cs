using Api.Responces.Global;
using Domain.Generals;

namespace Api.Responces.Globals;

public class ErrorResponce : BaseResponce
{
    public Error? Error { get; set; }
}