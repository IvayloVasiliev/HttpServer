using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class UserRegisterInputModel
    {
        private const string UsernameErrorMassege = "Invalid username length!" +
            " Must be between 5 and 20 symbols";

        [RequiredSis]
        [StringLengthSis(5, 20, "UsernameErrorMassege")]
        public string Username { get; set; }

        [RequiredSis]
        [PasswordSis(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [RequiredSis]
        public string ConfirmPassword { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }

    }
}
