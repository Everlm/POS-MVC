namespace POS_MVC.ApplicationWeb.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Role { get; set; }
        public int? IsActive { get; set; }

    }
}
