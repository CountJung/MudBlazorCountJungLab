using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace MudBlazorCountJungLab.Context
{
    public class GlobalContext
    {
        private readonly JavaScriptModuleService jsModuleService;
        private readonly ILogger<GlobalContext> logger;

        public GlobalContext(JavaScriptModuleService jsModuleService, ILogger<GlobalContext> logger)
        {
            this.jsModuleService = jsModuleService;
            this.logger = logger;
        }

        public bool DarkMode { get; set; }
        public string? User { get; set; } = "😃";
        public string? Profile { get; set; } = "";

        record ImageDemension(int Width, int Height);

        public async Task<string> GetImageSourceFromBase64FilePath(IBrowserFile file)
        {
            string imagesrc = "";
            try
            {
                long maxFileSize = 1024L * 1024L * 1024L * 2L;
                var streamRefernce = new DotNetStreamReference(file.OpenReadStream(maxFileSize));
                var jsModule = await jsModuleService.GetJsModuleAsync();
                var json = await jsModule.InvokeAsync<string>("getImageDimensions", streamRefernce);
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
            }
            return await Task.FromResult(string.Format("data:image/png;base64,{0}", imagesrc));
        }
    }
}
