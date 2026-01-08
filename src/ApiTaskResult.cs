using System.Text.Json;

namespace Opx.Http
{
    public class ApiTaskResult
    {
        public bool Result { get; set; }
        public dynamic? Data { get; set; }
        public string StatusCode { get; set; }
    }

    public static class ApiTaskExtension
    {
		public static T ConvertDataTo<T>(this ApiTaskResult source, bool breakException = true)
		{
			var result = default(T);

			if (source == null)
				return result;

			void Parsing()
			{
				var jsonString = Convert.ToString(source.Data);

				if (string.IsNullOrEmpty(jsonString))
					return;

				var typeName = typeof(T).Name;
				var sourceName = source.Data.GetType().Name;

				if (sourceName == "JsonElement")
				{
					result = (T)JsonSerializer.Deserialize<T>(source.Data);
				}
				else
				{
					switch (typeName)
					{
						case "String":
							result = source.Data.ToString();
							break;
						case "Int32":
							result = Convert.ToInt32(jsonString);
							break;
						case "Int64":
							result = Convert.ToInt64(jsonString);
							break;
						case "Decimal":
							result = Convert.ToDecimal(jsonString);
							break;
						case "Double":
							result = Convert.ToDouble(jsonString);
							break;
						case "Boolean":
							result = Convert.ToBoolean(jsonString);
							break;
						case "DateTime":
							result = Convert.ToDateTime(source.Data.ToString());
							break;
						case "System.Text.Json.JsonElement":
							result = JsonSerializer.Deserialize<T>(source.Data);
							break;
						default:
							result = (T)JsonSerializer.Deserialize<T>(jsonString,
						   new JsonSerializerOptions()
						   {
							   PropertyNameCaseInsensitive = true,
							   WriteIndented = false,
							   PropertyNamingPolicy = JsonNamingPolicy.CamelCase
						   });
							break;
					}
				}
			}

			if (breakException)
			{
				Parsing();
			}
			else
			{
				try
				{
					Parsing();
				}
				catch (Exception ex)
				{

				}
			}

			return result;
		}
	}
}