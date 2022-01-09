using Microsoft.JSInterop;

namespace ForEvolve.Blazor.WebStorage;

public sealed class LocalStorage : Storage, ILocalStorage
{
    public LocalStorage(IJSInProcessRuntime jsInProcessRuntime, IJSRuntime jsRuntime)
        : base(StorageType.Local, jsInProcessRuntime, jsRuntime) { }
}

public interface ILocalStorage : IStorage { }