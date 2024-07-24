namespace Domain.Generals
{
    public class ResultT<T> : Result
    {
        private readonly T? _value;
        public T? Value => _value;

        protected internal ResultT(T? value, string message = "") 
            : this(value, true, Error.NONE, message)
        {
            
        }
        protected internal ResultT(T? value, bool isSuccess, Error error,string message = "")
            : base(isSuccess,error,message)
        {
            _value = value;
        }


    }
}
