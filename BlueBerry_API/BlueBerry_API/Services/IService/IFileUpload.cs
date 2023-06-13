using Microsoft.AspNetCore.Components.Forms;

namespace BlueBerry_API.Services.IService
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IFormFile file);

        bool DeleteFile(string fileName);
    }
}
