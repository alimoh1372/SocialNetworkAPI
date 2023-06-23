

using Microsoft.AspNetCore.Http;

namespace _00_Framework.Application;

/// <summary>
/// Helping to upload files to server
/// </summary>
public interface IFileUpload
{
    /// <summary>
    /// Give the file <paramref name="file"/> cref="file"/> and the path of where to save <paramref name="path"/>
    /// <br/>
    /// and save the file into it
    /// </summary>
    /// <param name="file">a file to upload on server</param>
    /// <param name="path">path of where to save file the base path specify in implementing of <see cref="IFormFile"/></param>
    /// <returns><see langword="null"/> if it isn't success else return string of path from root of project </returns>
    string UploadFile(IFormFile file, string path);

    /// <summary>
    /// Delete a file from server 
    /// </summary>
    /// <param name="filePathWithoutRoot"><see langword="true"/> if file deleted else return false</param>
    /// <returns></returns>
    bool DeleteFile(string filePathWithoutRoot);
}
