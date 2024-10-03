namespace Opx.Http
{
    internal static class HttpHelper
    {
        public static Version GetVersionNumber(this HttpRequestVersion ver)
        {
            return ver switch
            {
                HttpRequestVersion.Http11 => new Version(1, 1),
                HttpRequestVersion.Http20 => new Version(2, 0),
                HttpRequestVersion.Http30 => new Version(3, 0),
                _=> new Version(1, 0)
            };
        }

        public static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        public static void RunSync(Func<Task> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}