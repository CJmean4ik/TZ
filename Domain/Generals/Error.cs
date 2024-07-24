namespace Domain.Generals
{
    public class Error
    {
        public static readonly Error NONE = new Error(ErrorCodes.NoError, string.Empty);
        public static readonly Error NULL_VALUE = new Error(ErrorCodes.ValueNull, "Value turned out to be null");

        public ErrorCodes Code { get; }
        public string Message { get; }

        public Error(ErrorCodes code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}