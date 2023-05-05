namespace POS_MVC.BLL.Interfaces
{
    public interface IUtilitiesService
    {
        string GeneratePassword();
        string ConverterSha256(string text);
    }
}
