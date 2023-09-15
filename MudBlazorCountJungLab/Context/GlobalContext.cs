using Microsoft.AspNetCore.Components.Forms;

namespace MudBlazorCountJungLab.Context
{
    public class GlobalContext
    {
        //public static GlobalVariable? Instance { get; set; }
        public GlobalContext()
        {
            //Instance = this;
        }

        public bool DarkMode { get; set; }

        public async Task<string> GetImageSourceFromBase64FilePath(IBrowserFile file)
        {
            var resizedFile = await file.RequestImageFileAsync(file.ContentType, 500, 500);
            var buffer = new byte[resizedFile.Size];
            using var streamData = resizedFile.OpenReadStream();
            await streamData.ReadAsync(buffer);
            var imagesrc = Convert.ToBase64String(buffer);
            return await Task.FromResult(string.Format("data:image/png;base64,{0}", imagesrc));
        }
    }
}
