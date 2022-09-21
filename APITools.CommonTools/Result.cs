namespace APITools.CommonTools
{
    public record Result
    {
        protected static readonly Result SuccessInstance = new(null);

#pragma warning disable CS8601
#pragma warning disable CS8618
        protected Result(Exception? exception)
        {
            IsSuccess = exception is null;
            Exception = exception;
        }
#pragma warning restore CS8601
#pragma warning restore CS8618

        public bool IsSuccess { get; }

        public Exception Exception { get; }

        public static implicit operator Result(Exception exception)
        {
            return Error(exception);
        }

        public static Result Success()
        {
            return SuccessInstance;
        }

        public static Result Error(Exception exception)
        {
            return new(exception);
        }

        public static Result<TResult> Success<TResult>(TResult value)
        {
            return new Result<TResult>(value);
        }

        public static Result<TResult> Error<TResult>(Exception exception)
        {
            return new(exception);
        }
    }

    public sealed record Result<TResult> : Result
    {
#pragma warning disable CS8601
#pragma warning disable CS8618
        internal Result(Exception exception) : base(exception)
        {
            Value = default;
        }

        internal Result(TResult value) : base(null)
        {
            Value = value;
        }
#pragma warning restore CS8601
#pragma warning restore CS8618

        public TResult Value { get; }

        public static implicit operator Result<TResult>(TResult value)
        {
            return Success(value);
        }

        public static implicit operator Result<TResult>(Exception exception)
        {
            return Error<TResult>(exception);
        }
    }
}
