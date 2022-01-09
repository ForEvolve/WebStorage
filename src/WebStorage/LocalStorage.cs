using Microsoft.JSInterop;

namespace ForEvolve.Blazor.WebStorage;

public sealed class LocalStorage : Storage
{
    public LocalStorage(IJSInProcessRuntime jsInProcessRuntime, IJSRuntime jsRuntime)
        : base(StorageType.Local, jsInProcessRuntime, jsRuntime) { }
}
