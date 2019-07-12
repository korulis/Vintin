using System;
using System.Threading.Tasks;

namespace Discounts.Monad
{
    public static class ResultExtensions
    {
        private static Result<string> GetResult(string value) => Result.Success(value);
        private static Task<string> GetTask(string value) => Task.FromResult(value);
        private static Task<Result<string>> GetTaskResult(string value) => Task.FromResult(Result.Success(value));

        public static Result<T> ToResult<T>(this T source, Func<IFailure> nullCaseFailure)
        {
            if (source == null)
                return Result.Fail<T>(nullCaseFailure());

            return Result.Success(source);
        }

        public static Result<T> ToResult<T>(this T source, IFailure nullCaseFailure)
        {
            return source.ToResult(() => nullCaseFailure);
        }

        public static async Task<Result<TResult>> Map<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Task<TResult>> projection)
        {
            switch (source)
            {
                case Success<TSource> success:
                    var result = await projection(success.Value);
                    return Result.Success(result);
                case Fail<TSource> fail:
                    return new Fail<TResult>(fail.Failure);
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, "Result has a different implementation that success or fail");
            }
        }

        public static async Task<Result<TResult>> Map<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, TResult> projection)
        {
            return (await source).Map(projection);
        }

        public static async Task<Result<TResult>> Map<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Task<TResult>> projection)
        {

            return await Map(await source, projection);
        }

        public static async Task<Result<TResult>> Bind<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Task<Result<TResult>>> binding)
        {
            switch (source)
            {
                case Success<TSource> success:
                    return await binding(success.Value);
                case Fail<TSource> fail:
                    return new Fail<TResult>(fail.Failure);
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, "Result has a different implementation that success or fail");
            }
        }

        public static async Task<Result<TResult>> Bind<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Result<TResult>> binding)
        {
            return (await source).Bind(binding);
        }

        public static async Task<Result<TResult>> Bind<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Task<Result<TResult>>> binding)
        {
            return await (await source).Bind(binding);
        }

        public static async Task<Result<TResult>> Bind<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Task<TResult>> binding)
        {
            return await source.Bind(async x =>
            {
                var result = await binding(x);
                return Result.Success(result);
            });
        }

        public static async Task<Result<TResult>> Bind<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Task<TResult>> binding)
        {
            return await (await source).Bind(binding);
        }

        public static async Task<Result<T>> Tee<T>(this Result<T> source, Func<T, Task> tee)
        {
            switch (source)
            {
                case Success<T> success:
                    await tee(success.Value);
                    return source;
                case Fail<T> fail:
                    return source;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, "Result has a different implementation that success or fail");
            }
        }

        public static async Task<Result<T>> Tee<T>(this Task<Result<T>> source, Action<T> tee)
        {
            return (await source).Tee(tee);
        }

        public static async Task<Result<T>> Tee<T>(this Task<Result<T>> source, Func<T, Task> tee)
        {
            return await (await source).Tee(tee);
        }
    }

    public static class ResultLinqExtensions
    {
        public static Result<TResult> Select<TSource, TResult>(this Result<TSource> source,
            Func<TSource, TResult> projection)
        {
            return source.Map(projection);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, TResult> projection)
        {
            return source.Map(projection);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Task<TResult>> projection)
        {
            return source.Map(projection);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Task<TResult>> projection)
        {
            return ResultExtensions.Map(source, projection);
        }

        public static Result<TResult> Select<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Result<TResult>> binding)
        {
            return source.Bind(binding);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Result<TResult>> binding)
        {
            return source.Bind(binding);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Result<TSource> source,
            Func<TSource, Task<Result<TResult>>> binding)
        {
            return source.Bind(binding);
        }

        public static Task<Result<TResult>> Select<TSource, TResult>(this Task<Result<TSource>> source,
            Func<TSource, Task<Result<TResult>>> binding)
        {
            return source.Bind(binding);
        }

        public static Result<V> SelectMany<T, U, V>(this Result<T> source, Func<T, Result<U>> func,
            Func<T, U, V> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }

        public static Result<V> SelectMany<T, U, V>(this Result<T> source, Func<T, Result<U>> func,
            Func<T, U, Result<V>> binding)
        {
            return source.Bind(a => func(a).Bind(b => binding(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Result<U>> func,
            Func<T, U, V> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Task<Result<U>>> func,
            Func<T, U, V> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }

        //        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Task<U>> func,
        //            Func<T, U, Result<V>> binding)
        //        {
        //            return source.Bind(a => func(a).Map(b => binding(a, b)));
        //        }

        //        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Task<U>> func,
        //            Func<T, U, V> projection)
        //        {
        //            return source.Bind(a => func(a).Map(b => projection(a, b)));
        //        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Task<Result<U>>> func,
            Func<T, U, V> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }


        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Task<V>> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Task<V>> projection)
        {
            return source.Bind(a => func(a).Map(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Result<U>> func,
            Func<T, U, Task<V>> projection)
        {
            return source.Bind(a => func(a).Select(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Result<U>> func,
            Func<T, U, Task<V>> projection)
        {
            return source.Bind(a => func(a).Select(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Result<U>> func,
            Func<T, U, Result<V>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Result<V>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Result<V>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Task<Result<V>>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Task<Result<U>>> func,
            Func<T, U, Task<Result<V>>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Result<T> source, Func<T, Result<U>> func,
            Func<T, U, Task<Result<V>>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }

        public static Task<Result<V>> SelectMany<T, U, V>(this Task<Result<T>> source, Func<T, Result<U>> func,
            Func<T, U, Task<Result<V>>> projection)
        {
            return source.Bind(a => func(a).Bind(b => projection(a, b)));
        }
    }
}