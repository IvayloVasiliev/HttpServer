using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class RegisterInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        [EmailSis("")]
        public string Email { get; set; }

    }
}
