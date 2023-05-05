namespace POS_MVC.BLL.Interfaces
{
    public interface IFireBaseService
    {
        Task<string> UploadStorageAsync(Stream FileStream, string DestinationFolder, string FileName);
        Task<bool> DeleteStorageAsync(string DestinationFolder, string FileName);
    }
}
