# Opx.HttpClient
HTTP Client tools, simplify HttpClient.

[![NuGet version (Opx.HttpClient)](https://img.shields.io/nuget/v/Opx.HttpClient.svg?style=flat-square)](https://www.nuget.org/packages/Opx.HttpClient/)

## Usage

### Basic
```c#
var api = new ApiRequest("https://localhost");

// https://localhost/customers
var customers = await api.RequestJsonAsync<List<Customer>>(HttpMethod.Post, "customers");
```

### Parameter From Query
```c#
// https://localhost/customers?Active=true&Sort=name
var customers = await api.RequestJsonAsync<List<Customer>>(HttpMethod.Get, "customers", new ApiRequestParameter()
{
    FromQuery = new
    {
        Active = true,
        Sort = "name"
    }
);
```
### Parameter From Route
```c#
// https://localhost/customers/1
var customer = await api.RequestJsonAsync<Customer>(HttpMethod.Get, "customers/{Id}", new ApiRequestParameter()
{
    FromRoute = new
    {
        Id = 1
    }
);
```

### Parameter From Body
```c#
// https://localhost/customers/save
var success = await api.RequestJsonAsync<bool>(HttpMethod.Post, "customers/save", new ApiRequestParameter()
{
    FromBody = new
    {
        Id = 1,
        FirstName = "John",
        LastName = "Doe"
    }
);
```
You can mix those parameters together.

## HTTP Request Version
Default `RequestVersion` is HTTP/2
```C#
var doc = new DocRequest("https://tools.keycdn.com/http2-test", HttpRequestVersion.Http20);
var res = doc.RequestString(HttpMethod.Get);
```
For HTTP/3 you must set `httpVersionPolicy` to `HttpVersionPolicy.RequestVersionExact`
```c#
var doc = new DocRequest("https://cloudflare-quic.com", HttpRequestVersion.Http30, HttpVersionPolicy.RequestVersionExact);
var res = doc.RequestString(HttpMethod.Get);
```


