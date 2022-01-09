using Microsoft.JSInterop;

namespace ForEvolve.Blazor.WebStorage;

public sealed class SessionStorage : Storage, ISessionStorage
{
    public SessionStorage(IJSInProcessRuntime jsInProcessRuntime, IJSRuntime jsRuntime)
        : base(StorageType.Session, jsInProcessRuntime, jsRuntime) { }
}

public interface ISessionStorage : IStorage { }