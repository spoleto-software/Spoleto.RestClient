# Spoleto.RestClient

[![](https://img.shields.io/github/license/spoleto-software/Spoleto.RestClient)](https://github.com/spoleto-software/Spoleto.RestClient/blob/main/LICENSE)
[![](https://img.shields.io/nuget/v/Spoleto.RestClient)](https://www.nuget.org/packages/Spoleto.RestClient/)
![Build](https://github.com/spoleto-software/Spoleto.RestClient/actions/workflows/ci.yml/badge.svg)

## Overview

Spoleto.RestClient is a flexible and easy-to-use wrapper around `HttpClient` designed to simplify making HTTP requests and handling responses in various formats: JSON, XML and Binary in .NET applications.  
The client also supports authentication and provides the ability for flexible customization and overriding of base classes.  
It supports various .NET versions, including .NET 7, .NET 8, and NetStandard 2.0.  
This library aims to provide a comfortable, convenient, and powerful way to interact with HTTP-based APIs.

https://github.com/spoleto-software/Spoleto.RestClient

## Features

- **Flexible and Customizable**: Easily configure and extend to meet your specific needs.
- **User-Friendly**: Intuitive API design that simplifies making HTTP requests.
- **Supports Authentication**: Built-in support for both static and dynamic request authentication.
- **Multi-Targeting**: Compatible with .NET 7, .NET 8, and NetStandard 2.0.

## Quick setup

Begin by installing the package through the [NuGet](https://www.nuget.org/packages/Spoleto.RestClient/) package manager with the command:

```sh
dotnet add package Spoleto.RestClient
```

## Usage

### Creating a RestClient

To create an instance of `RestHttpClient`, use the `RestClientFactory`:

```csharp
var restClient = new RestClientFactory()
    .WithHttpClient(new HttpClient())
    .WithAuthenticator(new StaticAuthenticator("Bearer", "your-static-token"))
    .Build();
```

### Authentication

#### Static Authenticator

For static tokens:

```csharp
var authenticator = new StaticAuthenticator("Bearer", "your-static-token");
```

#### Dynamic Authenticator

For dynamic tokens, extend the `DynamicAuthenticator` class, e.g.:

```csharp
public class MyAuthenticator : DynamicAuthenticator
{
    public const string TokenTypeName = "Bearer";

    private readonly AuthCredentials _authCredentials;

    public MyAuthenticator(AuthCredentials authCredentials) : base(TokenTypeName)
    {
        _authCredentials = authCredentials;    
    }

    protected override async Task<string> GetAuthenticationToken(IRestClient client)
    {
        var restRequest = new RestRequestFactory(RestHttpMethod.Post, "oauth/token")
            .WithFormUrlEncodedContent(_authCredentials)
            .Build();

        var authResponseModel = await client.ExecuteAsync<AuthToken>(restRequest).ConfigureAwait(false);
        return authResponseModel.AccessToken;
    }
}
```

### Creating Requests

The `RestRequestFactory` makes it easy to build requests:

- **With Query String**:

```csharp
.WithQueryString(new { param1 = "value1", param2 = "value2" });
// or
.WithQueryString("arg1=va1&arg2=val2");
// or
var model = GetModel();
.WithQueryString(model);
```

- **With JSON Content**:

```csharp
.WithJsonContent(new { property = "value" });
// or
var model = GetModel();
.WithJsonContent(model);
```

- **With application/x-www-form-urlencoded**:
```csharp
Dictionary<string, string> data = GetData();
.WithFormUrlEncodedContent(data);
// or
var model = GetModel();
.WithFormUrlEncodedContent(model);
```

- **With Headers**:

```csharp
.WithHeader("Custom-Header", "HeaderValue");
```

- **With Bearer Token**:

```csharp
.WithBearerToken("your-token");
```

### Examples how to send requests

#### GET Request with Query Parameters

```csharp
public async Task<List<City>> GetCitiesAsync(CityRequest cityRequest)
{
    var restRequest = new RestRequestFactory(RestHttpMethod.Get, "cities")
        .WithQueryString(cityRequest)
        .Build();

    var cityList = await _restClient.ExecuteAsync<List<City>>(restRequest).ConfigureAwait(false);
    return cityList;
}
```

#### POST Request with JSON Content

```csharp
public async Task<Order> CreateOrderAsync(OrderRequest orderRequest)
{
    var restRequest = new RestRequestFactory(RestHttpMethod.Post, "orders")
        .WithJsonContent(orderRequest)
        .Build();

    var deliveryOrder = await _masterPostClient.ExecuteAsync<DeliveryOrder>(restRequest).ConfigureAwait(false);
    return deliveryOrder;
}
```

## IRestClient Interface

The base interface for making requests:

```csharp
public interface IRestClient : IDisposable
{
    Task<TextRestResponse> ExecuteAsStringAsync(RestRequest request, CancellationToken cancellationToken = default);
    Task<BinaryRestResponse> ExecuteAsBytesAsync(RestRequest request, CancellationToken cancellationToken = default);
    Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class;
}
```

## Creating Custom RestClient

Spoleto.RestClient is designed to be easily extensible, allowing you to create your own custom `RestClient` to fit specific needs.  
Below is an example of how you can create a custom `RestClient` by inheriting from `RestHttpClient`:

```csharp
public class MyClient : RestHttpClient
{
    private readonly MyOptions _myOptions;

    public MyClient(MyOptions myOptions, AuthCredentials authCredentials)
        : this(myOptions, CreateNewClient(myOptions), CreateAuthenticator(authCredentials), RestClientOptions.Default, true)
    {
    }

    public MyClient(MyOptions myOptions, HttpClient httpClient, IAuthenticator? authenticator = null, RestClientOptions? options = null, bool disposeHttpClient = false)
        : base(httpClient, authenticator, options, disposeHttpClient)
    {
        _myOptions = myOptions;
    }

    private static HttpClient CreateNewClient(MyOptions myOptions)
    {
        // It's up to you to use Polly here in HttpMessageHandler:
        var httpClient = new HttpClient { BaseAddress = new Uri(myOptions.ServiceUrl) };
        return httpClient;
    }

    private static IAuthenticator CreateAuthenticator(AuthCredentials authCredentials)
    {
        var authenticator = new MyAuthenticator(authCredentials);
        return authenticator;
    }
}
```

In this example, `MyClient` is a custom `RestHttpClient` that can be configured with specific options and authentication credentials. This allows you to tailor the HTTP client to your particular needs, such as integrating with specific services or adding custom authentication mechanisms.

## Serialization and Deserialization

All serialization and deserialization in Spoleto.RestClient are handled by the `SerializationManager`.  
The default JSON serializer is based on `System.Text.Json`, the defaul XML serializer is based on `System.Xml.Serialization.XmlSerializer` and you can easily adjust the serializers to fit your needs.

### SerializationManager

Methods of `SerializationManager`:

```csharp
    public static T Deserialize<T>(IRestResponse restResponse) where T : class;

    public static T Deserialize<T>(string raw);
	
	public static string? Serialize<T>(DataFomat dataFormat, T? value) where T : class;
```

### Adjusting SerializationManager

You can easily customize the `SerializationManager` by modifying the `Serializers` property:

```C#
	public static SerializerCollection Serializers { get; }
```

### Example: Replacing the Default JSON Serializer

By default, `SerializationManager` uses a JSON serializer based on `System.Text.Json`. If you want to replace it with a different JSON serializer, such as Newtonsoft.Json, you can do so by modifying the `Serializers` collection:

```csharp
SerializationManager.Serializers.RemoveType<JsonSerializer>();
SerializationManager.Serializers.Add(new NewtonsoftJsonSerializer());
```

Here's an example of what a custom serializer might look like:

```csharp
public class NewtonsoftJsonSerializer : IJsonSerializer
{
    public bool CanDeserialize(IRestResponse response)
    {
        return response.ContentType.Contains("application/json");
    }

    public bool CanDeserialize(string raw)
    {
        // Implement a check to see if the raw string is JSON
    }

    public T Deserialize<T>(IRestResponse response) where T : class
    {
        return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public T Deserialize<T>(string raw) where T : class
    {
        return JsonConvert.DeserializeObject<T>(raw);
    }

    public string Serialize<T>(T value) where T : class
    {
        return JsonConvert.SerializeObject(value);
    }
}
```

## Conclusion

Spoleto.RestClient simplifies HTTP requests in .NET by providing a user-friendly and flexible wrapper around `HttpClient`. Whether you need to perform simple GET requests or complex POST requests with authentication, Spoleto.RestClient offers a clean and easy-to-use API to get the job done.
