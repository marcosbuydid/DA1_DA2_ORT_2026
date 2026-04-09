
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = " New password is required.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = " Retyped new password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string RetypedNewPassword { get; set; }

        public ChangePasswordDTO(string oldPassword, string newPassword, string retypedNewPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
            RetypedNewPassword = retypedNewPassword;
        }
    }
}
