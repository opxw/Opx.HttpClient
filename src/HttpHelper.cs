using System.Text.Json;

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

        public static T? ConvertFromJson<T>(this ApiTaskResult s, bool breakOnException = true)
        {
            var result = default(T);

            if (s == null)
                return result;

            void Parse()
            {
                var jsonString = System.Convert.ToString(s.Data);

                if (string.IsNullOrEmpty(jsonString))
                    return;

                var typeName = typeof(T).Name;

                switch (typeName)
                {
                    case "String":
                        result = s.Data.ToString();
                        break;
                    case "Int32":
                        result = System.Convert.ToInt32(jsonString);
                        break;
                    case "Int64":
                        result = System.Convert.ToInt64(jsonString);
                        break;
                    case "Decimal":
                        result = System.Convert.ToDecimal(jsonString);
                        break;
                    case "Double":
                        result = System.Convert.ToDouble(jsonString);
                        break;
                    case "Boolean":
                        result = System.Convert.ToBoolean(jsonString);
                        break;
                    case "DateTime":
                        result = System.Convert.ToDateTime(s.Data.ToString());
                        break;
                    default:
                        result = (T)JsonSerializer.Deserialize<T>(jsonString,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true,
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        break;
                }
            }

            if (breakOnException)
            {
                Parse();
            }
            else
            {
                try
                {
                    Parse();
                }
                catch (Exception ex)
                {
                }
            }

            return result;
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