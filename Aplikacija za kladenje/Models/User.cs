//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Aplikacija_za_kladenje.Models
//{
//    public class User
//    {
//        public int UserId { get; set; }
//        [Required(ErrorMessage ="This field is required.")]
//        public string Name { get; set; }
//        [Required(ErrorMessage = "This field is required.")]
//        public string Surname { get; set; }
//        [Required(ErrorMessage = "This field is required.")]
//        [Range(18, 100)]
//        public int Age { get; set; }
//        [Required(ErrorMessage = "This field is required.")]
//        [StringLength(35)]
//        public string City { get; set; }
//        [Required(ErrorMessage = "This field is required.")]
//        public string Address { get; set; }
//        [Required(ErrorMessage = "This field is required.")]
//        public string UserName { get; set; }
//        [Required]
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password")]
//        public string Password { get; set; }
//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm password")]
//        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//        public string ConfirmPassword { get; set; }
//    }
//}
