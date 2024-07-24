namespace Domain.Generals
{
    public class Result
    {
        public string Message { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected internal Result() { }

        protected internal Result(bool isSuccess, Error error,string message = "")
        {
            if (isSuccess && error != Error.NONE)           
                throw new InvalidOperationException("It is not possible to initialize the success of an operation while transmitting error information");

            if (!isSuccess && error == Error.NONE)          
                throw new InvalidOperationException("It is impossible to initialize the failure of an operation while transmitting without having an error");

            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }

        public static Result Success(string message = "") => new Result(true, Error.NONE,message);
        public static Result Failure(Error error) => new Result(false, error);

        public static ResultT<TValue> Success<TValue>(TValue value, string message = "") => new ResultT<TValue>(value, true, Error.NONE,message);
        public static ResultT<TValue> Failure<TValue>(Error error) => new ResultT<TValue>(default, false, error);

    }
}
