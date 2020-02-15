using System.ComponentModel.DataAnnotations;

namespace BettingApplication.ViewModels
{
    public class RoleVM
    {
        [Required]
        public string RoleName { get; set; }
    }
}
