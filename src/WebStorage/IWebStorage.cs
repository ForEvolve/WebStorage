namespace ForEvolve.Blazor.WebStorage;

public interface IWebStorage
{
    IStorage LocalStorage { get; }
    IStorage SessionStorage { get; }
    //onstorage event
}
