namespace POS_MVC.ApplicationWeb.Utilities.Response
{
    public class GenericResponse<TObject>
    {
        public bool State { get; set; }
        public string? Message { get; set; }
        public TObject? Object { get; set; }
        public List<TObject>? ListObjects { get; set; }
    }
}
