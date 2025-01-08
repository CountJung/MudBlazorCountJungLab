using Microsoft.JSInterop;
using System.Threading.Tasks;

public class JavaScriptModuleService : IAsyncDisposable
{
    private readonly IJSRuntime jsRuntime;
    private IJSObjectReference? jsModule;

    public JavaScriptModuleService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task<IJSObjectReference> GetJsModuleAsync()
    {
        if (jsModule == null)
        {
            jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/util.js");
        }
        return jsModule;
    }

    public async ValueTask DisposeAsync()
    {
        if (jsModule != null)
        {
            await jsModule.DisposeAsync();
        }
    }
}
