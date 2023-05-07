namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class MenuViewModel
    {

        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Controller { get; set; }
        public string? PageAction { get; set; }
        public virtual ICollection<MenuViewModel> InverseParentMenu { get; set; }
    }
}
