using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.NetworkInformation;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazorCountJungLab.Provider;

namespace MudBlazorCountJungLab.Context
{
    public class GlobalContext
    {
        private IJSObjectReference? jsModule;
        private readonly IJSRuntime JS;
        private readonly ILogger<GlobalContext> logger;
        //[Inject] IJSRuntime
        public GlobalContext(IJSRuntime runtime, ILogger<GlobalContext> logger)
        {
            JS = runtime;
            _ = LoadJSModule();
            this.logger = logger;
        }

        private async Task LoadJSModule()
        {
            jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/util.js");
        }

        public bool DarkMode { get; set; }
        public string? User { get; set; } = "😃";
        public string? Profile { get; set; } = "";
        record ImageDemension(int Width, int Height);
        public async Task<string> GetImageSourceFromBase64FilePath(IBrowserFile file)
        {
            string imagesrc="";
            try
            {
                long maxFileSize = 1024L * 1024L * 1024L * 2L;
                var streamRefernce = new DotNetStreamReference(file.OpenReadStream(maxFileSize));
                var json = await jsModule!.InvokeAsync<string>("getImageDimensions", streamRefernce);
                var imageInfo = JsonSerializer.Deserialize<ImageDemension>(json);
                var resizedFile = await file.RequestImageFileAsync(file.ContentType, imageInfo!.Width, imageInfo!.Height);
                var buffer = new byte[resizedFile.Size];
                using var streamData = resizedFile.OpenReadStream(maxFileSize);
                await streamData.ReadAsync(buffer);
                imagesrc = Convert.ToBase64String(buffer);
                return await Task.FromResult(string.Format("data:image/png;base64,{0}", imagesrc));
            }
            catch (IOException ioEx)
            {
                logger.LogCritical(ioEx.ToString());
            }
            catch(Exception ex) 
            {
                logger.LogCritical(ex.ToString());
            }
            return await Task.FromResult(string.Format("data:image/png;base64,{0}", imagesrc));
        }

    }
}
