using System;

namespace Discounts.Monad
{
    public static class Result
    {
        public static Result<TResult> Success<TResult>(TResult result)
        {
            return new Success<TResult>(result);
        }

        public static Result<TResult> Fail<TResult>(IFailure failure)
        {
            return new Fail<TResult>(failure);
        }
    }

    public interface Result<TResult>
    {
        Result<TMapped> Map<TMapped>(Func<TResult, TMapped> projection);
        Result<TResult> MapFail(Func<IFailure, IFailure> failureProjection);
        Result<TMapped> Bind<TMapped>(Func<TResult, Result<TMapped>> binding);
        Result<TResult> BindFail(Func<IFailure, Result<TResult>> failBinding);
        Result<TResult> Tee(Action<TResult> tee);
        Result<TResult> TeeFail(Action<IFailure> tee);
        TResolved MatchWith<TResolved>(Func<TResult, TResolved> success, Func<IFailure, TResolved> fail);
    }

    public class Success<TResult> : Result<TResult>
    {
        public TResult Value { get; }

        public Success(TResult value)
        {
            Value = value;
        }

        public Result<TMapped> Map<TMapped>(Func<TResult, TMapped> projection)
        {
            return new Success<TMapped>(projection(Value));
        }

        public Result<TResult> MapFail(Func<IFailure, IFailure> failureProjection)
        {
            return this;
        }

        public Result<TMapped> Bind<TMapped>(Func<TResult, Result<TMapped>> binding)
        {
            return binding(Value);
        }

        public Result<TResult> BindFail(Func<IFailure, Result<TResult>> failBinding)
        {
            return this;
        }

        public Result<TResult> Tee(Action<TResult> tee)
        {
            tee(Value);
            return this;
        }

        public Result<TResult> TeeFail(Action<IFailure> tee)
        {
            return this;
        }

        public TResolved MatchWith<TResolved>(Func<TResult, TResolved> success, Func<IFailure, TResolved> fail)
        {
            return success(Value);
        }
    }

    public class Fail<TResult> : Result<TResult>
    {
        public IFailure Failure { get; }

        public Fail(IFailure failure)
        {
            Failure = failure;
        }

        public Result<TMapped> Map<TMapped>(Func<TResult, TMapped> projection)
        {
            return new Fail<TMapped>(Failure);
        }

        public Result<TResult> MapFail(Func<IFailure, IFailure> failureProjection)
        {
            return new Fail<TResult>(failureProjection(Failure));
        }

        public Result<TMapped> Bind<TMapped>(Func<TResult, Result<TMapped>> binding)
        {
            return new Fail<TMapped>(Failure);
        }

        public Result<TResult> BindFail(Func<IFailure, Result<TResult>> failBinding)
        {
            return failBinding(Failure);
        }

        public Result<TResult> Tee(Action<TResult> tee)
        {
            return this;
        }

        public Result<TResult> TeeFail(Action<IFailure> tee)
        {
            tee(Failure);
            return this;
        }

        public TResolved MatchWith<TResolved>(Func<TResult, TResolved> success, Func<IFailure, TResolved> fail)
        {
            return fail(Failure);
        }
    }

    public interface IFailure
    {
        string Message { get; }
    }
}