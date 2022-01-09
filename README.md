# WebStorage

WebStorage is a simple Blazor WebAssembly (Wasm) Web Storage API wrapper. It implements both session storage and local storage.

# How to install?

| Name                                             | NuGet.org                                                                                                                                  | feedz.io                                                                                                                                                                                                                                                     |
| ------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `dotnet add package ForEvolve.Blazor.WebStorage` | [![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.Blazor.WebStorage)](https://www.nuget.org/packages/ForEvolve.Blazor.WebStorage/) | [![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fwebstorage%2Fshield%2FForEvolve.Blazor.WebStorage%2Flatest)](https://f.feedz.io/forevolve/webstorage/packages/ForEvolve.Blazor.WebStorage/latest/download) |

# Usage

There are multiple ways to use WebStorage:

-   Use the default `IStorage` implementation.
-   Use the `IWebStorage` API that exposes both `LocalStorage` and `SessionStorage`.
-   Use the `LocalStorage` or `SessionStorage` classes directly.
-   Use the `ILocalStorage` or `ISessionStorage` marker interfaces.

## Register WebStorage with the IoC container

To register Web Storage with the IoC container, in your `Program.cs` file, add the following lines:

```csharp
using ForEvolve.Blazor.WebStorage;
//...
builder.Services.AddWebStorage();
//...
```

## Default IStorage

An instance of the `LocalStorage` class is bound to the `IStorage` interface by default.
You can inject `IStorage` in your classes, like this:

```csharp
public class MyService
{
    private readonly IStorage _storage;
    public MyService(IStorage storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }
    // Omitted implementation
}
```

### Change the default IStorage binding

To change the default `IStorage` binding, you must set the `DefaultStorageType` property of the `WebStorageOptions` to `StorageType.Session`.

Example:

```csharp
builder.Services.AddWebStorage(options => options.DefaultStorageType = StorageType.Session);
```

## Use the IWebStorage API

If you want to access both `SessionStorage` and `LocalStorage`, you can inject `IWebStorage` in your classes, like this:

```csharp
public class MyService
{
    private readonly IWebStorage _webStorage;
    public MyService(IWebStorage webStorage)
    {
        _webStorage = webStorage ?? throw new ArgumentNullException(nameof(webStorage));
    }

    public void SomeMethod(){
        var localStorage = _webStorage.LocalStorage;
        var sessionStorage = _webStorage.SessionStorage;
    }
}
```

## Use the implementation directly

You can also directly inject the `LocalStorage` and `SessionStorage` implementation into your classes, like this:

```csharp
public class MyService1
{
    private readonly LocalStorage _localStorage;
    public MyService1(LocalStorage localStorage)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }
    // Omitted implementation
}
public class MyService2
{
    private readonly SessionStorage _sessionStorage;
    public MyService2(SessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
    }
    // Omitted implementation
}
public class MyService3
{
    private readonly LocalStorage _localStorage;
    private readonly SessionStorage _sessionStorage;

    public MyService3(LocalStorage localStorage, SessionStorage sessionStorage)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
    }
    // Omitted implementation
}
```

## Use the marker interfaces

If you don't want to directly inject the implementations, you can leverage the `ILocalStorage` and `ISessionStorage` marker interfaces instead.
This yields the same result as injecting the classes directly but allows for more flexibility; you could create decorators or mocks for example.

```csharp
public class MyService1
{
    private readonly ILocalStorage _localStorage;
    public MyService1(ILocalStorage localStorage)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }
    // Omitted implementation
}
public class MyService2
{
    private readonly ISessionStorage _sessionStorage;
    public MyService2(ISessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
    }
    // Omitted implementation
}
public class MyService3
{
    private readonly ILocalStorage _localStorage;
    private readonly ISessionStorage _sessionStorage;

    public MyService3(ILocalStorage localStorage, ISessionStorage sessionStorage)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
    }
    // Omitted implementation
}
```

## The IStorage API

The `IStorage` interface implements synchronous and asynchronous versions of the Web Storage JavaScript API.
Both `LocalStorage` and `SessionStorage` classes implement the `IStorage` interface.

```csharp
public interface IStorage
{
    /// <summary>
    /// Gets the number of data items stored in a given <see cref="IStorage"/> object.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Returns the name of the nth key in a given <see cref="IStorage"/> object.
    /// The order of keys is user-agent defined, so you should not rely on it.
    /// </summary>
    /// <param name="index">
    /// The number of the key you want to get the name of.
    /// This is a zero-based index.
    /// </param>
    /// <returns>
    /// The name of the key. If the index does not exist, null is returned.
    /// </returns>
    string? Key(int index);

    /// <summary>
    /// Returns the specified key's value, or null if the key does not exist, in the
    /// given <see cref="IStorage"/> object.
    /// </summary>
    /// <param name="keyName">The name of the key you want to retrieve the value of.</param>
    /// <returns>The value of the key. If the key does not exist, null is returned.</returns>
    string? GetItem(string keyName);

    /// <summary>
    /// Adds the specified key to the given <see cref="IStorage"/> object, or
    /// updates that key's value if it already exists.
    /// </summary>
    /// <param name="keyName">The name of the key you want to create/update.</param>
    /// <param name="keyValue">The value you want to give the key you are creating/updating.</param>
    /// <remarks>
    /// setItem() may throw an exception if the storage is full. Particularly, in
    /// Mobile Safari (since iOS 5) it always throws when the user enters private
    /// mode. (Safari sets the quota to 0 bytes in private mode, unlike other browsers,
    /// which allow storage in private mode using separate data containers.) Hence
    /// developers should make sure to always catch possible exceptions from setItem().
    /// </remarks>
    void SetItem(string keyName, string keyValue);

    /// <summary>
    /// Removes the specified key from the given <see cref="IStorage"/> object if it exists.
    /// </summary>
    /// <param name="keyName">The name of the key you want to remove.</param>
    void RemoveItem(string keyName);

    /// <summary>
    /// Clears all keys stored in a given <see cref="IStorage"/> object.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets the number of data items stored in a given <see cref="IStorage"/> object.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The number of data items stored in the <see cref="IStorage"/> object.</returns>
    ValueTask<int> GetLengthAsync(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Returns the name of the nth key in a given <see cref="IStorage"/> object.
    /// The order of keys is user-agent defined, so you should not rely on it.
    /// </summary>
    /// <param name="index">
    /// The number of the key you want to get the name of.
    /// This is a zero-based index.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// The name of the key. If the index does not exist, null is returned.
    /// </returns>
    ValueTask<string?> KeyAsync(int index, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Returns the specified key's value, or null if the key does not exist, in the
    /// given <see cref="IStorage"/> object.
    /// </summary>
    /// <param name="keyName">The name of the key you want to retrieve the value of.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The value of the key. If the key does not exist, null is returned.</returns>
    ValueTask<string?> GetItemAsync(string keyName, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Adds the specified key to the given <see cref="IStorage"/> object, or
    /// updates that key's value if it already exists.
    /// </summary>
    /// <param name="keyName">The name of the key you want to create/update.</param>
    /// <param name="keyValue">The value you want to give the key you are creating/updating.</param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// setItem() may throw an exception if the storage is full. Particularly, in
    /// Mobile Safari (since iOS 5) it always throws when the user enters private
    /// mode. (Safari sets the quota to 0 bytes in private mode, unlike other browsers,
    /// which allow storage in private mode using separate data containers.) Hence
    /// developers should make sure to always catch possible exceptions from setItem().
    /// </remarks>
    ValueTask SetItemAsync(string keyName, string keyValue, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Removes the specified key from the given <see cref="IStorage"/> object if it exists.
    /// </summary>
    /// <param name="keyName">The name of the key you want to remove.</param>
    /// <param name="cancellationToken"></param>
    ValueTask RemoveItemAsync(string keyName, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Clears all keys stored in a given <see cref="IStorage"/> object.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask ClearAsync(CancellationToken? cancellationToken = default);
}
```

> The `IStorage` descriptions are based on the [Storage interface](https://developer.mozilla.org/en-US/docs/Web/API/Storage) documentation written by [Mozilla Contributors](https://developer.mozilla.org/en-US/docs/MDN/About/contributors.txt).

## The IStorage extensions

A few `IStorage` extensions allow serializing and deserializing values automatically, so you can get and set items directly.

The default serializer is `JsonWebStorageSerializer` which leverage the `System.Text.Json.JsonSerializer` class.
You can control its behavior by configuring the `JsonWebStorageSerializerOptions` object using .NET options pattern.

### Customize serialization

You can implement the `IWebStorageSerializer` interface to customize the serialization process.

# Found a bug or have a feature request?

Please open an issue and be as clear as possible; see _How to contribute?_ for more information.

# How to contribute?

If you would like to contribute to the project, first, thank you for your interest, and please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CONTRIBUTING.md) for more information.

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/Toc/blob/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.

# Origin & Similar project

This project started because I needed access to the storage API in an object with a Singleton lifetime.
So I opened [Web Storage API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API) page on MDN, and created this project.

Here are some alternatives that I know about:

-   [Blazored SessionStorage](https://github.com/Blazored/SessionStorage)
-   [Blazored LocalStorage](https://github.com/Blazored/LocalStorage)
