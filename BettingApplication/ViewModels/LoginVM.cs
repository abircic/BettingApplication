using System.ComponentModel.DataAnnotations;

namespace BettingApplication.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "This field is required.")]
        //[Remote(action: "IsUsernameExist", controller:"Account")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
